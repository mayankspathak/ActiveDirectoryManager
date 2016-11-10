using ActiveDirectoryManager.DomainManager;
using ActiveDirectoryManager.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Xml.Linq;

namespace ActiveDirectoryManager.Users
{
    /// <summary>
    /// Interacts with Active Directory
    /// </summary>
    internal sealed class ActiveDirectoryUser
    {
        #region  placeholders
        /// <summary>
        /// Domain Name placeholder
        /// </summary>
        public string _domainName;

        /// <summary>
        /// User Principal placeholder
        /// </summary>
        public UserPrincipal _userPrincipal;
        #endregion

        #region Properties
        /// <summary>
        /// Domain name 
        /// </summary>
        public string DomainName
        {
            get
            {
                return _domainName;
            }
        }

        /// <summary>
        /// User Principal 
        /// </summary>
        public UserPrincipal UserPrincipal
        {
            get
            {
                return this._userPrincipal;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="userPrincipal"></param>
        public ActiveDirectoryUser(string domainName, UserPrincipal userPrincipal)
        {
            this._domainName = domainName;
            this._userPrincipal = userPrincipal;
        }


        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        internal static IList<UserPrincipal> Get(string samAccountName, string displayName)
        {
            if (samAccountName == null && displayName == null)
                throw new ArgumentException("At least one parameter is required.");
            List<UserPrincipal> userPrincipalList = new List<UserPrincipal>();
            UserPrincipal userPrincipal1 = new UserPrincipal(new PrincipalContext(ContextType.Domain, DomainsManager.GetContextDomain()));
            if (samAccountName != null)
            {
                Utils.validateLength(samAccountName);
                userPrincipal1.SamAccountName = samAccountName;
            }
            if (displayName != null)
            {
                Utils.validateLength(displayName);
                userPrincipal1.DisplayName = displayName;
            }
            using (PrincipalSearcher principalSearcher = new PrincipalSearcher((Principal)userPrincipal1))
            {
                foreach (UserPrincipal userPrincipal2 in principalSearcher.FindAll())
                    userPrincipalList.Add(userPrincipal2);
            }
            return (IList<UserPrincipal>)userPrincipalList;
        }

        /// <summary>
        /// Filters Sam Account Name
        /// </summary>
        /// <param name="principalContext"></param>
        /// <param name="samAccountName"></param>
        /// <returns></returns>
        internal static UserPrincipal newFilterSamAccountName(PrincipalContext principalContext, string samAccountName)
        {
            UserPrincipal userPrincipal = new UserPrincipal(principalContext);
            string str = samAccountName;
            userPrincipal.SamAccountName = str;
            return userPrincipal;
        }

        /// <summary>
        /// New Filter Display Name
        /// </summary>
        /// <param name="principalContext"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        internal static UserPrincipal newFilterDisplayName(PrincipalContext principalContext, string displayName)
        {
            UserPrincipal userPrincipal = new UserPrincipal(principalContext);
            string str = displayName;
            userPrincipal.DisplayName = str;
            return userPrincipal;
        }

        /// <summary>
        /// Gets standard flow
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="xName"></param>
        /// <param name="newFilterPrincipal"></param>
        /// <param name="ActiveDirectoryUsers"></param>
        /// <returns></returns>
        internal static ICollection<ActiveDirectoryUser> standardFlow(string domainName, string xName, DomainsManager.NewFilterPrincipal newFilterPrincipal, ConcurrentDictionary<string, ActiveDirectoryUser> ActiveDirectoryUsers)
        {
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, domainName);
            using (PrincipalSearcher principalSearcher = new PrincipalSearcher((Principal)newFilterPrincipal(principalContext, xName)))
            {
                foreach (UserPrincipal userPrincipal in principalSearcher.FindAll())
                {
                    if (!ActiveDirectoryUsers.ContainsKey(userPrincipal.Sid.Value))
                        ActiveDirectoryUsers.TryAdd(userPrincipal.Sid.Value, new ActiveDirectoryUser(principalContext.Name, userPrincipal));
                }
            }
            return ActiveDirectoryUsers.Values;
        }

        /// <summary>
        /// Get properties
        /// </summary>
        /// <param name="directoryEntryProperties"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        internal static XElement getProperty(PropertyCollection directoryEntryProperties, string property)
        {
            if (directoryEntryProperties.Contains(property))
                return new XElement((XName)property, (object)directoryEntryProperties[property].Value.ToString());
            return new XElement((XName)property, (object)"");
        }

        /// <summary>
        /// ToXElement
        /// </summary>
        /// <param name="userPrincipal"></param>
        /// <returns></returns>
        internal static XElement ToXElement(UserPrincipal userPrincipal)
        {
            DirectoryEntry directoryEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
            return new XElement((XName)"UserPrincipal", new object[15]
            {
        (object) new XElement((XName) "GivenName", (object) userPrincipal.GivenName),
        (object) new XElement((XName) "Surname", (object) userPrincipal.Surname),
        (object) new XElement((XName) "EmailAddress", (object) userPrincipal.EmailAddress),
        (object) new XElement((XName) "VoiceTelephoneNumber", (object) userPrincipal.VoiceTelephoneNumber),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "Mobile"),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "Info"),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "Title"),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "PhysicalDeliveryOfficeName"),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "Department"),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "CostCenter"),
        (object) ActiveDirectoryUser.getProperty(directoryEntry.Properties, "Company"),
        (object) new XElement((XName) "Description", (object) userPrincipal.Description),
        (object) new XElement((XName) "DisplayName", (object) userPrincipal.DisplayName),
        (object) new XElement((XName) "EmployeeId", (object) userPrincipal.EmployeeId),
        (object) new XElement((XName) "SamAccountName", (object) userPrincipal.SamAccountName)
            });
        }

        /// <summary>
        /// ToXElement
        /// </summary>
        /// <returns></returns>
        internal XElement ToXElement()
        {
            XElement xelement1 = ActiveDirectoryUser.ToXElement(this._userPrincipal);
            XElement xelement2 = new XElement((XName)"DomainName", (object)this._domainName);
            xelement1.Add((object)xelement2);
            return xelement1;
        }


    }
}
