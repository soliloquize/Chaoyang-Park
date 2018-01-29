# -*- coding: utf-8 -*-

import os, sys, shutil
import platform, ctypes
from PythonTools import print_red
from PythonTools import print_green

# 配置路径
path = os.path.split(os.path.realpath(__file__))[0]
path_git_excel = path + '/../Table'
path_git_json = path + '/../Assets/Tab'

# argv
path_svn = ''
if len(sys.argv) > 1:
    path_svn = sys.argv[1]

# 配置路径
path_svn_excel = path_svn + '/server_tools/table'
path_svn_json = path_svn + '/server_tools/json'

record_git_excel = {}
record_git_json = {}

# 筛选合法文件
def filter_files(files, extname):
    filter = []
    for file in files:
        name, ext = os.path.splitext(file)
        if ext == extname and name.find('~$') == -1:
            filter.append(file) 
    return filter

# 
def record():
    for root, dirs, files in os.walk(path_git_excel):
        for xlsx in filter_files(files, '.xlsx'):
            record_git_excel[xlsx] = root + '/' + xlsx

    for root, dirs, files in os.walk(path_git_json):
        for json in filter_files(files, '.json'):
            record_git_json[json] = root + '/' + json

# 
def copy_xlsx():
    for root, dirs, files in os.walk(path_svn_excel):
        for xlsx in filter_files(files, '.xlsx'):
            if xlsx in record_git_excel.keys():
                shutil.copy(root + '/' + xlsx, record_git_excel[xlsx])
                print '<Modify>................' + xlsx
            else:
                shutil.copy(root + '/' + xlsx, path_git_excel)
                print '<Add>...................' + xlsx

    print ''

#
def copy_json():
    for root, dirs, files in os.walk(path_svn_json):
        for json in filter_files(files, '.json'):
            if json in record_git_json.keys():
                shutil.copy(root + '/' + json, record_git_json[json])
                print '<Modify>................' + json
            else:
                shutil.copy(root + '/' + json, path_git_json)
                print '<Add>...................' + json

# Main
if path_svn == '':
    print_red('[Not Available Svn Path]')
else:
    record()
    #copy_xlsx()
    copy_json()
    print_green('[Finish]')

if(platform.system() == 'Windows'):
    raw_input(unicode('按回车键退出...','utf-8').encode('gbk'))