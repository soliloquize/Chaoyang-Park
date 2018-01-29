# -*- coding: utf-8 -*-
#!/bin/sh

PACKAGE_NAME=$1
PLATFORM=$2
VERSION_NUMBER=$3

UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
#PROJECT_PATH=/Volumes/Resource/Projects/gungirls/GunGirls

PROJECT_PATH=$(cd "$(dirname "$0")"; pwd)
PROJECT_PATH=${PROJECT_PATH}/../

ANDROID_PATH=${PROJECT_PATH}/${PLATFORM}
#BUILD_PATH=${ANDROID_PATH}/build
BACKUP_PATH=${PROJECT_PATH}/../../Backup/${PACKAGE_NAME}/${PLATFORM}
VERSION_PATH=${PROJECT_PATH}/../../Version/${PACKAGE_NAME}/${PLATFORM}
PACKAGE_PATH=${PROJECT_PATH}/${PLATFORM}/

#if [[  -d "$ANDROID_PATH" ]]; then
#	rm -f -r ${ANDROID_PATH}
#fi

$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildPackage project-${PACKAGE_NAME} platform-${PLATFORM} -batchmode -quit

BACKUP_PATH=${BACKUP_PATH}/${VERSION_NUMBER}_$(date "+%Y-%m-%d_%H-%M")

if [[ ! -d ${BACKUP_PATH} ]]; then
	mkdir -p ${BACKUP_PATH}
fi

cp -f ${PACKAGE_PATH}${PACKAGE_NAME}.apk ${BACKUP_PATH}/${PACKAGE_NAME}_${VERSION_NUMBER}.apk

if [[ -d ${VERSION_PATH} ]]; then
	rm -f -r ${VERSION_PATH}
fi

mkdir -p ${VERSION_PATH}
mv -f ${PACKAGE_PATH}${PACKAGE_NAME}.apk ${VERSION_PATH}/${PACKAGE_NAME}_${VERSION_NUMBER}.apk

echo -e "\033[5;32;40m"
echo $1" APK Generate Finish."
echo -e "\033[0m"
