using ActiveDirectoryManager.Configurations;
using ActiveDirectoryManager.Users;
using ActiveDirectoryManager.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading;

namespace ActiveDirectoryManager.DomainManager
{
    /// <summary>
    /// Domains Manager
    /// </summary>
    internal class DomainsManager
    {
        internal delegate UserPrincipal NewFilterPrincipal(PrincipalContext principalContext, string xName);

        internal delegate ICollection<ActiveDirectoryUser> ExecAsync(string domainName, string xName, NewFilterPrincipal newFilterPrincipal, ConcurrentDictionary<string, ActiveDirectoryUser> ActiveDirectoryUsers);

        /// <summary>
        /// Gets domain name
        /// </summary>
        /// <returns></returns>
        internal static string GetContextDomain()
        {
            return Configuration.GetAppSetting("AdMgr.ContextDomain" + ".Default");
        }

        /// <summary>
        /// Gets All domains from comma separated list
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetContextDomains()
        {
            string settingAllowNull = Configuration.GetAppSettingAllowNull("AdMgr.ContextDomains");
            List<string> stringList = new List<string>();
            char[] chArray = new char[1] { ',' };
            foreach (string str in settingAllowNull.Split(chArray))
                stringList.Add(Configuration.GetAppSetting(string.Format("{0}.{1}", (object)"AdMgr.ContextDomain", (object)str.Trim())));
            return stringList;
        }

        /// <summary>
        /// Gets where domain
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="samAccountName"></param>
        /// <returns></returns>
        internal static ICollection<UserPrincipal> GetWhereDomain(string domainName, string samAccountName)
        {
            if (domainName == null || samAccountName == null)
                throw new ArgumentNullException();
            List<UserPrincipal> userPrincipalList = new List<UserPrincipal>();
            UserPrincipal userPrincipal1 = new UserPrincipal(new PrincipalContext(ContextType.Domain, domainName));
            Utils.validateLength(samAccountName);
            string str = samAccountName;
            userPrincipal1.SamAccountName = str;
            using (PrincipalSearcher principalSearcher = new PrincipalSearcher((Principal)userPrincipal1))
            {
                foreach (UserPrincipal userPrincipal2 in principalSearcher.FindAll())
                    userPrincipalList.Add(userPrincipal2);
            }
            return (ICollection<UserPrincipal>)userPrincipalList;
        }

        /// <summary>
        /// Get Multiple Domains
        /// </summary>
        /// <param name="xName"></param>
        /// <returns></returns>
        internal static ICollection<ActiveDirectoryUser> GetMultipleDomains(string xName)
        {
            if (string.IsNullOrWhiteSpace(xName))
                throw new ArgumentNullException("xName");
            Utils.validateLength(xName);
            List<string> contextDomains = DomainsManager.GetContextDomains();
            ConcurrentDictionary<string, ActiveDirectoryUser> ActiveDirectoryUsers = new ConcurrentDictionary<string, ActiveDirectoryUser>();
            List<WaitHandle> waitHandleList = new List<WaitHandle>();

            ExecAsync execAsync = new ExecAsync(ActiveDirectoryUser.standardFlow);
            foreach (string domainName in contextDomains)
            {
                waitHandleList.Add(execAsync.BeginInvoke(domainName, xName, new DomainsManager.NewFilterPrincipal(ActiveDirectoryUser.newFilterSamAccountName), ActiveDirectoryUsers, (AsyncCallback)null, (object)null).AsyncWaitHandle);
                waitHandleList.Add(execAsync.BeginInvoke(domainName, xName, new DomainsManager.NewFilterPrincipal(ActiveDirectoryUser.newFilterDisplayName), ActiveDirectoryUsers, (AsyncCallback)null, (object)null).AsyncWaitHandle);
            }
            WaitHandle.WaitAll(waitHandleList.ToArray());
            return ActiveDirectoryUsers.Values;
        }
    }
}
