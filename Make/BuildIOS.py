# -*- coding: utf-8 -*-
#!/bin/sh

if [ $# != 1 ];then  
    echo "需要一个参数作为游戏包名。"  
    exit     
fi

UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
PROJECT_PATH=/Volumes/Resource/Projects/gungirls/GunGirls
XCODE_PATH=${PROJECT_PATH}/iOS
BUILD_IOS_PATH=${PROJECT_PATH}/Make/BuildIOS.sh

$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildIOS project-$1 -batchmode -quit
 
echo "Xcode工程生成完毕"

$BUILD_IOS_PATH $XCODE_PATH/$1 $1

echo "IPA生成完毕"