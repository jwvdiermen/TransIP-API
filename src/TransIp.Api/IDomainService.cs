using System.Net;
using TransIp.Api.Dto;
using TransIp.Api.Remote;
using DnsEntry = TransIp.Api.Dto.DnsEntry;
using Domain = TransIp.Api.Dto.Domain;
using DomainAction = TransIp.Api.Dto.DomainAction;
using DomainCheckResult = TransIp.Api.Dto.DomainCheckResult;
using Nameserver = TransIp.Api.Dto.Nameserver;
using Tld = TransIp.Api.Dto.Tld;
using WhoisContact = TransIp.Api.Dto.WhoisContact;

namespace TransIp.Api
{
	/// <summary>
	/// The service for domain related tasks.
	/// </summary>
	public interface IDomainService
	{
		/// <summary>
		/// Checks the availability of multiple domains.
		/// </summary>
		/// <param name="domainNames">The domain names to check for availability. A maximum of 20 domainNames at once can be checked.</param>
		/// <returns>A list of DomainCheckResult objects, holding the domainName and the status per result.</returns>
		DomainCheckResult[] BatchCheckAvailability(string[] domainNames);

		/// <summary>
		/// Checks the availability of a domain.
		/// </summary>
		/// <param name="domainName">The domain name to check for availability</param>
		/// <returns>The availability status of the domain name.</returns>
		AvailabilityStatus CheckAvailability(string domainName);

		/// <summary>
		/// Gets the whois of a domain name.
		/// </summary>
		/// <param name="domainName">The domain name to get the whois for.</param>
		/// <returns>The whois data for the domain.</returns>
		string GetWhois(string domainName);

		/// <summary>
		/// Gets the names of all domains in your account.
		/// </summary>
		/// <returns>A list of all domains in your account.</returns>
		string[] GetDomainNames();

		/// <summary>
		/// Gets information about a domainName.
		/// </summary>
		/// <param name="domainName">The domain name to get the information for.</param>
		/// <returns>A domain object holding the data for the requested domainName.</returns>
		Domain GetInfo(string domainName);

		/// <summary>
		/// Registers a domain name, will automatically create and sign a proposition for it.
		/// </summary>
		/// <param name="domain">The domain object holding information about the domain that needs to be registered.</param>
		/// <remarks>Requires "readwrite" mode.</remarks>
		void Register(Domain domain);

		/// <summary>
		/// Cancels a domain name, will automatically create and sign a cancellation document.
		/// Please note that domains with webhosting cannot be cancelled through the API.
		/// </summary>
		/// <param name="domainName">The domain name that needs to be cancelled.</param>
		/// <param name="endTime">The time to cancel the domain.</param>
		void Cancel(string domainName, CancellationTime endTime);

		/// <summary>
		/// Transfers a domain with changing the owner, not all TLDs support this (e.g. nl).
		/// </summary>
		/// <param name="domain">The Domain object holding information about the domain that needs to be transfered.</param>
		/// <param name="authCode">The authorization code for domains needing this for transfers (e.g. .com or .org transfers). Leave empty when n/a.</param>
		void TransferWithOwnerChange(Domain domain, string authCode);

		/// <summary>
		/// Transfers a domain without changing the owner.
		/// </summary>
		/// <param name="domain">The Domain object holding information about the domain that needs to be transfered.</param>
		/// <param name="authCode">The authorization code for domains needing this for transfers (e.g. .com or .org transfers). Leave empty when n/a.</param>
		void TransferWithoutOwnerChange(Domain domain, string authCode);

		/// <summary>
		/// Starts a nameserver change for this domain, will replace all existing nameservers with the new nameservers.
		/// </summary>
		/// <param name="domainName">The domain name to change the nameservers for.</param>
		/// <param name="nameservers">The list of new nameservers for this domain.</param>
		void SetNameservers(string domainName, Nameserver[] nameservers);

		/// <summary>
		/// Lock this domain in real time.
		/// </summary>
		/// <param name="domainName">The domain name to set the lock for.</param>
		void SetLock(string domainName);

		/// <summary>
		/// Unlocks this domain in real time.
		/// </summary>
		/// <param name="domainName">The domain name to unlock.</param>
		void UnsetLock(string domainName);

		/// <summary>
		/// Sets the DnEntries for this Domain, will replace all existing dns entries with the new entries.
		/// </summary>
		/// <param name="domainName">The domain mame to change the dns entries for.</param>
		/// <param name="dnsEntries">The list of new DnsEntries for this domain.</param>
		void SetDnsEntries(string domainName, DnsEntry[] dnsEntries);

		/// <summary>
		/// Starts an owner change of a Domain, brings additional costs with the following TLDs:
		/// .nl
		/// .be
		/// .eu
		/// </summary>
		/// <param name="domainName">The domainName to change the owner for.</param>
		/// <param name="registrantWhoisContact">The new contact data for this.</param>
		void SetOwner(string domainName, WhoisContact registrantWhoisContact);

		/// <summary>
		/// Starts a contact change of a domain, this will replace all existing contacts.
		/// </summary>
		/// <param name="domainName">The domainName to change the contacts for.</param>
		/// <param name="contacts">The list of new contacts for this domain.</param>
		void SetContacts(string domainName, WhoisContact[] contacts);

		/// <summary>
		/// Get TransIP supported TLDs.
		/// </summary>
		/// <returns>Array of Tld objects.</returns>
		Tld[] GetAllTldInfos();

		/// <summary>
		/// Get info about a specific TLD.
		/// </summary>
		/// <param name="tldName">The tld to get information about.</param>
		/// <returns>Tld object with info about this Tld.</returns>
		Tld GetTldInfo(string tldName);

		/// <summary>
		/// Gets info about the action this domain is currently running.
		/// </summary>
		/// <param name="domainName">Name of the domain.</param>
		/// <returns>If this domain is currently running an action, a corresponding DomainAction with info about the action will be returned.</returns>
		DomainAction GetCurrentDomainAction(string domainName);

		/// <summary>
		/// Retries a failed domain action with new domain data. The Domain#name field must contain
		/// the name of the Domain, the nameserver, contacts, dnsEntries fields contain the new data for this domain.
		/// Set a field to null to not change the data.
		/// </summary>
		/// <param name="domain">The domain with data to retry.</param>
		void RetryCurrentDomainActionWithNewData(Domain domain);

		/// <summary>
		/// Retry a transfer action with a new authcode.
		/// </summary>
		/// <param name="domain">The domain to try the transfer with a different authcode for.</param>
		/// <param name="newAuthCode">New authorization code to try.</param>
		void RetryTransferWithDifferentAuthCode(Domain domain, string newAuthCode);

		/// <summary>
		/// Cancels a failed domain action.
		/// </summary>
		/// <param name="domain">The domain to cancel the action for.</param>
		void CancelDomainAction(Domain domain);

		/// <summary>
		/// Gets the service client.
		/// </summary>
		DomainServicePortTypeClient Client { get; }

		/// <summary>
		/// Gets the name of the service.
		/// </summary>
		string ServiceName { get; set; }

		/// <summary>
		/// Gets the login.
		/// </summary>
		string Login { get; }

		/// <summary>
		/// Gets the mode.
		/// </summary>
		ClientMode Mode { get; }

		/// <summary>
		/// Gets the cookie container.
		/// </summary>
		CookieContainer Cookies { get; }
	}
}
