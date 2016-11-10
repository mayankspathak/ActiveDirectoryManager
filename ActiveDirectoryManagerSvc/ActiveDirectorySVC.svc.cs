using ActiveDirectoryManager;
using System;
using System.ServiceModel;
using System.Xml.Linq;

namespace ActiveDirectoryManagerSvc
{
    /// <summary>
    /// Service Class for Active Directory Service
    /// This WCF Service will be exposed on IIS and can be used to interact with Active directory
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ActiveDirectorySVC : IActiveDirectorySVC
    {
        /// <summary>
        /// Get User Principal
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public XElement Get(string text)
        {
            XElement userPrincipals = new XElement((XName)"UserPrincipals");
            try
            {
                string str = string.Format("*{0}*", (object)text);
                ActiveDirectory.GetUser(str, (string)null, userPrincipals);
                ActiveDirectory.GetUser((string)null, str, userPrincipals);
                return userPrincipals;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        /// <summary>
        /// Get Where SAM Account Name is
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="samAccountName"></param>
        /// <returns></returns>
        public XElement GetWhereDomainSamAccountName(string domainName, string samAccountName)
        {
            XElement userPrincipals = new XElement((XName)"UserPrincipals");
            try
            {
                ActiveDirectory.GetUserWhereDomain(domainName, samAccountName, userPrincipals);
                return userPrincipals;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        /// <summary>
        /// Get Multiple Domain names
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public XElement GetMultipleDomains(string text)
        {
            XElement userPrincipals = new XElement((XName)"UserPrincipals");
            try
            {
                ActiveDirectory.GetUserMultipleDomains(string.Format("*{0}*", (object)text), userPrincipals);
                return userPrincipals;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        /// <summary>
        /// Get All computers
        /// </summary>
        /// <returns></returns>
        public XElement GetAllComputers()
        {
            XElement computerPrincipals = new XElement((XName)"ComputerPrincipals");
            try
            {
                ActiveDirectory.GetAllComputers(computerPrincipals);
                return computerPrincipals;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        /// <summary>
        /// Executer delegate
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="xName"></param>
        private delegate void ExecAsync(string domainName, string xName);
    }
}
