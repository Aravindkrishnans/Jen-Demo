def APP_NAME = "PAN_Label_App_Test"
def Site = "Pan_Test_App"
def Port = "8024"
def WORKSPACE = "F:\\Aravind\\jenkins01\\workspace"
def MSBUILD = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2022\\BuildTools\\MSBuild\\Current\\Bin\\"
def File =  "dir /b *.sln"
//def File = bat (script:"dir($WORKSPACE\\${env.JOB_NAME}\\$APP_NAME\\){dir /b *.sln}", returnStdout: true)
def APPCMD = "C:\\Windows\\System32\\inetsrv\\"
def Path = "$WORKSPACE\\${env.JOB_NAME}\\Build\\_PublishedWebsites\\"
pipeline{
    agent {node {label "INFORDEV"}}
    stages{
        stage('checkout'){
            steps{
                checkout([$class: 'SubversionSCM',locations: [[cancelProcessOnExternalsFail: true, credentialsId: 'svn', depthOption: 'infinity', ignoreExternalsOption: true, local: "$APP_NAME", remote: 'http://10.1.1.11:8092/svn/PanLabelsTest']]])
                echo "Checkout Sucess"
            }
        }
        stage('Build'){
            steps{
                //APP_Name = bat (script:"$File")
                dir("$WORKSPACE\\${env.JOB_NAME}"){
                    bat "IF EXIST Build (echo Exist) ELSE mkdir Build"
                }
                dir ("$MSBUILD"){
                    script{
                    dir("$WORKSPACE\\${env.JOB_NAME}\\${APP_NAME}\\"){
                        //echo "Start"
                        APP_Name = bat (returnStdout: true,script:"dir /B *.sln").trim()
                        result = APP_Name.readLines().drop(1).join(" ")
                    }
                    
                }
                    bat "MSBuild $WORKSPACE\\${env.JOB_NAME}\\$APP_NAME\\${result} -t:Build,Publish -p:outdir=$WORKSPACE\\${env.JOB_NAME}\\Build"
                }
                
                echo "Build Sucess"
            }
        }
        stage('Deploy'){
            steps{
                script{
                    if ("${env.BUILD_NUMBER}" >= 1)
                    {
                        dir("$APPCMD"){
                            bat "appcmd.exe stop site $Site && appcmd.exe start site $Site"
                        }
                    }
                    else{
                        dir("$APPCMD") {
                            script{
                                dir("$Path"){
                                    test = bat(returnStdout:true,script: "dir /b").trim()
                                    result = test.readLines().drop(1).join(" ")
                                    echo "$result"
                                    //bat "dir /b"
                                }
                                bat "appcmd.exe add site /name:$Site /bindings:http/*:$Port: /physicalpath:$Path${result}"
                            }
                        }
                    }
                }
            }
        }
    }
}




