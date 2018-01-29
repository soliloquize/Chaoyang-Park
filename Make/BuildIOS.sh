# -*- coding: utf-8 -*-
#!/bin/sh

PACKAGE_NAME=$1
PLATFORM=$2
VERSION_NUMBER=$3

UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
#PROJECT_PATH=/Volumes/Resource/Projects/gungirls/GunGirls

PROJECT_PATH=$(cd "$(dirname "$0")"; pwd)
PROJECT_PATH=${PROJECT_PATH}/../

XCODE_PATH=${PROJECT_PATH}${PLATFORM}/${PACKAGE_NAME}
BUILD_PATH=${XCODE_PATH}/build
BACKUP_PATH=${PROJECT_PATH}../../Backup/${PACKAGE_NAME}/${PLATFORM}
VERSION_PATH=${PROJECT_PATH}../../Version/${PACKAGE_NAME}/${PLATFORM}

if [[ -d ${XCODE_PATH} ]]; then
	rm -f -r ${XCODE_PATH}
fi

echo $PACKAGE_NAME" Xcode Project Generate Begin."

$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildPackage project-${PACKAGE_NAME} platform-${PLATFORM} -batchmode -quit

echo -e "\033[5;32;40m"
echo $PACKAGE_NAME" Xcode Project Generate Finish."
echo -e "\033[0m"

cd $XCODE_PATH

xcodebuild clean -project ${XCODE_PATH}/Unity-iPhone.xcodeproj -alltargets
xcodebuild archive -project ${XCODE_PATH}/Unity-iPhone.xcodeproj -scheme "Unity-iPhone" -archivePath ${BUILD_PATH}/${PACKAGE_NAME}.xcarchive
xcodebuild -exportArchive -archivePath ${BUILD_PATH}/${PACKAGE_NAME}.xcarchive -exportPath ${BUILD_PATH}/${PACKAGE_NAME} -exportFormat ipa -exportWithOriginalSigningIdentity 'iPhone Distribution: Juan Chen'
#xcrun -sdk iphoneos PackageApplication -v ${BUILD_PATH}/${PACKAGE_NAME}.xcarchive/Products/Applications/*.app -o ${BUILD_PATH}/${PACKAGE_NAME}.ipa

BACKUP_PATH=${BACKUP_PATH}/${VERSION_NUMBER}_`date "+%Y-%m-%d_%H-%M"`

if [[ ! -d ${BACKUP_PATH} ]]; then
	mkdir -p ${BACKUP_PATH}
fi

#cp -f -r ${BUILD_PATH}/${PACKAGE_NAME}.xcarchive ${BACKUP_PATH}/${PACKAGE_NAME}.xcarchive
#cp -f ${BUILD_PATH}/${PACKAGE_NAME}.ipa ${BACKUP_PATH}/../${PACKAGE_NAME}_${VERSION_NUMBER}.ipa

mv -f ${BUILD_PATH}/${PACKAGE_NAME}.xcarchive ${BACKUP_PATH}/${PACKAGE_NAME}.xcarchive

if [[ -d ${VERSION_PATH} ]]; then
	rm -f -r ${VERSION_PATH}
fi

mkdir -p ${VERSION_PATH}
mv -f ${BUILD_PATH}/${PACKAGE_NAME}.ipa ${VERSION_PATH}/${PACKAGE_NAME}_${VERSION_NUMBER}.ipa

echo -e "\033[5;32;40m"
echo $PACKAGE_NAME" IPA Generate Finish."
echo -e "\007\033[0m"
