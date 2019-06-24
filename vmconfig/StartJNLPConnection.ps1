add-type @"
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    public class TrustAllCertsPolicy : ICertificatePolicy {
        public bool CheckValidationResult(
            ServicePoint srvPoint, X509Certificate certificate,
            WebRequest request, int certificateProblem) {
            return true;
        }
    }
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$result = Invoke-WebRequest -Uri https://jenkins-w7/jnlpJars/slave.jar -Outfile "C:\Users\QAComp\Downloads\slave.jar" 

cd C:\Users\QAComp\Downloads
java -jar slave.jar -jnlpUrl https://jenkins-w7/computer/GRW10-64-A/slave-agent.jnlp -noCertificateCheck -secret 954fb9d5ddf7d4fa2b5f3cd0669add2879d422adb2104e53cc5765c2a23f6062







