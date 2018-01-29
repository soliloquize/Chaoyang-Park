# -*- coding: utf-8 -*-
#!/bin/sh

PACKAGE_NAME=$1
PLATFORM=$2
VERSION_NUMBER=$3

UNITY_PATH=/Applications/Unity/Unity.app/Contents/MacOS/Unity
#PROJECT_PATH=/Volumes/Resource/Projects/gungirls/GunGirls

PROJECT_PATH=$(cd "$(dirname "$0")"; pwd)
PROJECT_PATH=${PROJECT_PATH}/../

#WINDOWS_PATH=${PROJECT_PATH}/Windows
#BUILD_PATH=${WINDOWS_PATH}/build
BACKUP_PATH=${PROJECT_PATH}/../../Backup/${PACKAGE_NAME}/${PLATFORM}
VERSION_PATH=${PROJECT_PATH}/../../Version/${PACKAGE_NAME}/${PLATFORM}
PACKAGE_PATH=${PROJECT_PATH}/${PLATFORM}/${PACKAGE_NAME}

$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildWindows project-${PACKAGE_NAME} platform-${PLATFORM} -batchmode -quit

if [[ ! -d ${PACKAGE_PATH} ]]; then
	mkdir -p ${PACKAGE_PATH}
fi

mv -f ${PACKAGE_PATH}.exe ${PACKAGE_PATH}
mv -f ${PACKAGE_PATH}"_Data" ${PACKAGE_PATH}

cd $PACKAGE_PATH/
zip ${PACKAGE_PATH}.zip -q -X -9 -r .

#BACKUP_PATH=${BACKUP_PATH}/`date "+%Y-%m-%d_%H：%M：%S"`
BACKUP_PATH=${BACKUP_PATH}/${VERSION_NUMBER}_`date "+%Y-%m-%d_%H-%M"`

if [[ ! -d ${BACKUP_PATH} ]]; then
	mkdir -p ${BACKUP_PATH}
fi

cp -f ${PACKAGE_PATH}.zip ${BACKUP_PATH}/${PACKAGE_NAME}_${VERSION_NUMBER}.zip

if [[ -d ${VERSION_PATH} ]]; then
	rm -f -r ${VERSION_PATH}
fi

mkdir -p ${VERSION_PATH}
mv -f ${PACKAGE_PATH}.zip ${VERSION_PATH}/${PACKAGE_NAME}_${VERSION_NUMBER}.zip

if [[ -d ${PACKAGE_PATH} ]]; then
	rm -f -r ${PACKAGE_PATH}
fi

echo -e "\033[5;32;40m"
echo $1" EXE Generate Finish."
echo -e "\033[0m"
