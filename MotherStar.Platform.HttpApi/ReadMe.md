# Configurations

This project uses some configurations which are sensitive by nature. These configurations include database connection strings, username/passwords to third party services, 
etc. You will want to store such configurations in your secrets file. You can access your secrets file by right-clicking the 
"LeadVenture.SharedServices.Lighthouse.HttpApi project and then choosing "Manage Secrets".  Within that file, you will need to place something that looks like the following:

`{
  "ConnectionStrings": {
    "LighthouseDb": "Data Source=localhost;initial catalog=Lighthouse;Integrated Security=False;User ID=sa;Pwd=whatever;multipleactiveresultsets=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;application name=Lighthouse;"
  },
  "RabbitMQ": {
    "QueueSettings": {
      "HostName": "localhost",
      "VirtualHost": "/",
      "UserName": "guest",
      "Password": "whatever",
      "Port": "15672"
    }
  },
  "Lighthouse": {
    "ApiKey": "AIzaSyC-XY3dfsThisWillNotWork1zb34523mMu2ruY"
  }
}`

For the "Lighthouse:ApiKey", you will want to register a Google API client for the lighthouse 
app here: https://console.cloud.google.com/apis/library/pagespeedonline.googleapis.com. Copy and paste the API key in your secrets


For the APM to run, you will need to download APM Server: https://www.elastic.co/downloads/apm  . Then follow instructions in README.md from apm server directory
