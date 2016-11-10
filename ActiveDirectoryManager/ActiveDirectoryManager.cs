using ActiveDirectoryManager.Computers;
using ActiveDirectoryManager.DomainManager;
using ActiveDirectoryManager.Users;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Xml.Linq;

namespace ActiveDirectoryManager
{
    /// <summary>
    /// Active Directory Manager
    /// </summary>
    public sealed class ActiveDirectory
    {
        /// <summary>
        /// Gets users
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <param name="displayName"></param>
        /// <param name="userPrincipals"></param>
        public static void GetUser(string samAccountName, string displayName, XElement userPrincipals)
        {
            foreach (UserPrincipal userPrincipal in (IEnumerable<UserPrincipal>)ActiveDirectoryUser.Get(samAccountName, displayName))
                userPrincipals.Add((object)ActiveDirectoryUser.ToXElement(userPrincipal));
        }

        /// <summary>
        /// Gets users using domain name
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="samAccountName"></param>
        /// <param name="userPrincipals"></param>
        public static void GetUserWhereDomain(string domainName, string samAccountName, XElement userPrincipals)
        {
            foreach (UserPrincipal userPrincipal in (IEnumerable<UserPrincipal>)DomainsManager.GetWhereDomain(domainName, samAccountName))
                userPrincipals.Add((object)ActiveDirectoryUser.ToXElement(userPrincipal));
        }

        /// <summary>
        /// Get Multiple Domains
        /// </summary>
        /// <param name="xName"></param>
        /// <param name="userPrincipals"></param>
        public static void GetUserMultipleDomains(string xName, XElement userPrincipals)
        {
            foreach (ActiveDirectoryUser multipleDomain in (IEnumerable<ActiveDirectoryUser>)DomainsManager.GetMultipleDomains(xName))
                userPrincipals.Add((object)multipleDomain.ToXElement());
        }

        /// <summary>
        /// Gets all computers from Active directory
        /// </summary>
        /// <param name="computerPrincipals"></param>
        public static void GetAllComputers(XElement computerPrincipals)
        {
            foreach (ComputerPrincipal computerPrincipal in ActiveDirectoryComputers.GetAll())
                computerPrincipals.Add((object)ActiveDirectoryComputers.ToXElement(computerPrincipal));
        }
    }
}
