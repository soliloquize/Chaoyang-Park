# -*- coding: utf-8 -*-

import os, sys, shutil, platform
from PythonTools import print_green

# 配置路径
path = os.path.split(os.path.realpath(__file__))[0]
path_input = os.path.realpath(path + '/../Texture/UI')
path_output = os.path.realpath(path + '/../Assets/UI')

# 忽略列表
ignore_floders = []
#for line in open(path_input + '/_ignore.txt'):
#    ignore_floders.append(line.rstrip('\n'))

# 
def filter_floders(floders):
    filter = []
    for floder in floders:
        if not floder in ignore_floders:
            filter.append(floder) 
    return filter

# 
def start():
    for root, dirs, files in os.walk(path_input):
        for folder in filter_floders(dirs):
            pack(folder, root + '/' + folder, path_output + '/' + folder)
        break

def pack(folder, src, res):
    print '--------------------------------------------------'
    command = 'TexturePacker'
    command += ' --format unity'
    command += ' --disable-rotation'
    command += ' --opt RGBA8888'
    command += ' --size-constraints POT'
    command += ' --data ' + res + '.txt'
    command += ' --sheet ' + res + '.png'
    command += ' ' + src
    os.system(command)

os.system('TexturePacker --version')
start()
print_green('Finish')

if(platform.system() == 'Windows'):
    raw_input(unicode('按回车键退出...','utf-8').encode('gbk'))