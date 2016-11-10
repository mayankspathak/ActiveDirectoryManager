# ActiveDirectoryManager

Active Directory Manager is written in C#. This library allows you to interact with Active Directory.

How to Use ADM

Download the source code. Open it using Visual Studio.
You will find 2 projects.

1. ActiveDirectoryManager
2. ActiveDirectoryManagerSvc

1. ActiveDirectoryManager : This project's code let you interact with Active directory. you can open its Web.config and can '
                            edit "AdMgr.ContextDomain.Default" in AppSettings. Here you can configure your own Domain to search
                            using code. for example :  "internal.contoso.net"

2. ActiveDirectoryManagerSvc : This is the WCF Service, Which will consume "ActiveDirectoryManager" code to interact with Active 
                               Directory and expose. This Service must be installed on IIS or any other web server. 
                               
                               
Deployment : Please note, for deployment you must deploy ActiveDirectoryManager.dll along with the WCF Service on your webserver.
             


Kindly feel free to write me down on mayank0089@gmail.com for any suggestions or directly make changes to the source code. 


Happy Coding !!!



