In order to keep the VM's slave.jar's updated properly when have to download the slave.jar
before we connect to the master each time.

I have cretaed a powershell script to ingnore ssl warnings and download the slave.jar
then establish the jnlp connection.

Note you may need to install windows management framework update for Windows 7 and 8 in
order to use commands for powershell.

1. Create a node in Jenkins for the slave machine (I will come back to this later why it's important to do first)

2. Disable the user login process on the windows VM so that windows automatically logs in to the specified user.

https://www.intowindows.com/how-to-automatically-login-in-windows-10/

3. download the jnlp file for your specified salve node https://jenkins-w7/computer/(NAME OF NODE)/slave-agent.jnlp

4. Open the JNLP file up and copy the secret hash located between the carrot brackets. (ex. 954fb9d5ddf7d4fa2b5f3cd0669add2879d422adb2104e53cc5765c2a23f6062)

5. Edit the StartJNLPConnection.ps1 on the last line to point to the slave jnlp file loaction  https://jenkins-w7/computer/(NAME OF NODE)/slave-agent.jnlp and edit the secret has with what you copied from the last step.

6. Then you have to place the StartJNLPConnection.ps1 script in folder that will host the file and then edit the Start-Jnlp-Powershell.bat file to point to the location 

powershell -executionpolicy bypass -File  C:\Users\QAComp\Downloads\StartJNLPConnection.ps1

7. After you have edited and saved the file place the file In the startup folder so that it runs on startup.

8. Need to make sure that flags for browsers are set correctly in this case for chrome to disable the restore on crash popup you need to edit C:\Users\QAComp\AppData\Local\Google\Chrome\User Data\Default\Prefernces to be read-only and in firefox you need to disable the crash flag. Turn off Chrome process allowed to run in background.

9. Make sure all browsers and extensions are up to date and make sure to disable automatic updates.

You can test the script and the connection by running the batch file.

After you have set up your VM with all of this and the tools you need to build, a snapshot should be taken which is what you will use for the builder to start from scratch with (clean slate).