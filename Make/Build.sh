# -*- coding: utf-8 -*-
#!/bin/sh

if [ $# != 3 ];then
	echo -e "\033[5;31;42m"
    echo "Please Input Game Package Name & Game Platform & Version Number."
    echo -e "\007\033[0m"
    exit
fi

PACKAGE_NAME=$1
PLATFORM=$2
VERSION_NUMBER=$3

#SCRIPT_PATH=/Volumes/Resource/Projects/gungirls/GunGirls/Make
SCRIPT_PATH=$(cd "$(dirname "$0")"; pwd)
SCRIPT_PATH=${SCRIPT_PATH}/./

if [[ $PLATFORM = "iOS" ]]; then
	$SCRIPT_PATH/BuildIOS.sh $PACKAGE_NAME $PLATFORM $VERSION_NUMBER
elif [[ $PLATFORM = "Android" ]]; then
	$SCRIPT_PATH/BuildAndroid.sh $PACKAGE_NAME $PLATFORM $VERSION_NUMBER
elif [[ $PLATFORM = "Windows" ]]; then
	$SCRIPT_PATH/BuildWindows.sh $PACKAGE_NAME $PLATFORM $VERSION_NUMBER
elif [[ $PLATFORM = "Macintosh" ]]; then
	$SCRIPT_PATH/BuildMac.sh $PACKAGE_NAME $PLATFORM $VERSION_NUMBER
fi
