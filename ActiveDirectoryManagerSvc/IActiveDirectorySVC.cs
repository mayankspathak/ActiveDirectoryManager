using System.ServiceModel;
using System.Xml.Linq;

namespace ActiveDirectoryManagerSvc
{
    /// <summary>
    /// IActiveDirectory service contract
    /// </summary>
    [ServiceContract]
    public interface IActiveDirectorySVC
    {
        /// <summary>
        /// Gets User by Search text
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        [OperationContract]
        XElement Get(string Text);

        /// <summary>
        /// Gets where domain name with SAM accountname
        /// </summary>
        /// <param name="DomainName"></param>
        /// <param name="SamAccountName"></param>
        /// <returns></returns>
        [OperationContract]
        XElement GetWhereDomainSamAccountName(string DomainName, string SamAccountName);

        /// <summary>
        /// Gets multiple domains
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        [OperationContract]
        XElement GetMultipleDomains(string Text);

        /// <summary>
        /// Gets all computers
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        XElement GetAllComputers();
    }
}
