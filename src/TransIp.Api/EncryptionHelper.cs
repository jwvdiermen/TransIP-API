using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Encoder = Microsoft.Security.Application.Encoder;

namespace TransIp.Api
{
	/// <summary>
	/// Contains any method related to the encryption of API requests.
	/// </summary>
	public static class EncryptionHelper
	{
		private static readonly Regex PrivateKeyRegex =
			new Regex(@"-----BEGIN (RSA )?PRIVATE KEY-----(.*)-----END (RSA )?PRIVATE KEY-----",
				RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
		
		private static readonly Regex EscapeRegex = new Regex(@"%..", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// Signs the given arguments with the given private key.
		/// </summary>
		/// <param name="privateKey">The private key.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The signature.</returns>
		public static string Sign(string privateKey, object[] args)
		{
			var matches = PrivateKeyRegex.Matches(privateKey);
			if (matches.Count == 0)
			{
				throw new Exception("Invalid private key.");
			}

			/*var key = matches[0].Groups[2].Value;
			key = WhitespaceRegex.Replace(key, "");
			key = ChunkSplit(key, 64, "\n");
			key = "-----BEGIN RSA PRIVATE KEY-----\n" + key + "-----END RSA PRIVATE KEY-----";*/

			var digest = Sha512Asn1(EncodeArguments(args));
			var signature = Encrypt(digest, privateKey);

			return Convert.ToBase64String(signature);
		}

		private static string ChunkSplit(string input, int chunkLength, string end)
		{
			var sb = new StringBuilder();

			int position = 0;
			while (position < input.Length)
			{
				for (var i = 0; i < chunkLength && position < input.Length; ++i)
				{
					sb.Append(input[position++]);
				}

				if (position < input.Length)
				{
					sb.Append(end);
				}
			}

			sb.Append(end);
			return sb.ToString();
		}

		private static byte[] Sha512Asn1(string data)
		{
			var signature = new[]
			{
				0x30,
				0x51,
				0x30,
				0x0d,
				0x06,
				0x09,
				0x60,
				0x86,
				0x48,
				0x01,
				0x65,
				0x03,
				0x04,
				0x02,
				0x03,
				0x05,
				0x00,
				0x04,
				0x40
			};

			var hashAlg = SHA512.Create();
			var hash = hashAlg.ComputeHash(Encoding.ASCII.GetBytes(data));

			return signature.Select(x => (byte) x).Concat(hash).ToArray();
		}

		private static byte[] Encrypt(byte[] digest, string key)
		{
			var keyReader = new StringReader(key);
			var pemReader = new PemReader(keyReader);

			var pemObject = pemReader.ReadObject();
			ICipherParameters cipherParameters;
			if (pemObject is RsaPrivateCrtKeyParameters)
			{
				cipherParameters = (RsaPrivateCrtKeyParameters)pemObject;
			}
			else if (pemObject is AsymmetricCipherKeyPair)
			{
				var keyPair = (AsymmetricCipherKeyPair) pemObject;
				cipherParameters = keyPair.Private;
			}
			else
			{
				throw new Exception("Unsupported private key format. Got object of type '" + pemObject.GetType() + "' from PEM reader.");
			}

			var cipher = CipherUtilities.GetCipher("RSA/None/PKCS1Padding");
			cipher.Init(true, cipherParameters);

			return cipher.DoFinal(digest);
		}

		private static string EncodeArguments(object args, string keyPrefix = null)
		{
			if (!CanEnumerate(args))
			{
				return Encode(args);
			}

			var encodedData = new List<string>();
			foreach (var arg in Enumerate(args))
			{
				var encodedKey = keyPrefix == null ? Encoder.UrlEncode(arg.Key) : keyPrefix + "[" + Encoder.UrlEncode(arg.Key) + "]";

				if (CanEnumerate(arg.Value))
				{
					encodedData.Add(EncodeArguments(arg.Value, encodedKey));
				}
				else
				{
					encodedData.Add(encodedKey + "=" + Encode(arg.Value));
				}
			}

			return String.Join("&", encodedData);
		}

		/// <summary>
		/// Encodes the given object using its "ToString" implementation.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		public static string Encode(object obj)
		{
			var result = obj != null ? Encoder.UrlEncode(obj.ToString()) : "";
			result = result.Replace("%7E", "~"); // Not sure if this is necessary.
			result = EscapeRegex.Replace(result, match => { return match.Value.ToUpper(); });

			return result;
		}

		private static bool CanEnumerate(object arg)
		{
			if (arg == null)
			{
				return false;
			}
			if (arg is IEnumerable && !(arg is string))
			{
				return true;
			}
			if (arg.GetType().GetCustomAttributes(typeof (DataContractAttribute), false).Any())
			{
				return true;
			}

			return false;
		}

		private static IDictionary<string, object> Enumerate(object arg)
		{
			var result = new Dictionary<string, object>();

			if (arg is IEnumerable && !(arg is string))
			{
				int counter = 0;
				foreach (var obj in (IEnumerable) arg)
				{
					if (obj is KeyValuePair<string, string>)
					{
						var keyValuePair = (KeyValuePair<string, string>) obj;
						result.Add(keyValuePair.Key, keyValuePair.Value);
					}
					else
					{
						result.Add(counter.ToString(), obj);
					}
					counter++;
				}
			}
			else if (arg.GetType().GetCustomAttributes(typeof (DataContractAttribute), false).Any())
			{
				foreach (var member in arg.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance))
				{
					var attr = member.GetCustomAttribute<DataMemberAttribute>();
					if (attr != null)
					{
						result.Add(attr.Name ?? member.Name,
							member is FieldInfo ? ((FieldInfo) member).GetValue(arg) : ((PropertyInfo) member).GetValue(arg));
					}
				}
			}

			return result;
		}
	}
}