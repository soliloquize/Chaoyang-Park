# -*- coding: utf-8 -*-
import os, sys, platform
import json, xlrd, codecs, types

from collections import OrderedDict
from PythonTools import print_red
from PythonTools import print_green
from ConfigParser import ConfigParser
cf = ConfigParser()

# 解决终端外执行编码问题
reload(sys)
sys.setdefaultencoding('utf8')

# 配置路径
path = os.path.split(os.path.realpath(__file__))[0]
path_conf = path + '/../Assets/Editor/DataModel/DataModelGenerator.conf'
path_temp = path + '/../Assets/Editor/DataModel/DataModelTemplate.cs.txt'
path_xlsx = path + '/../Table'
path_json = path + '/../Assets/Tab'
path_data = path + '/../Assets/Script/DataModel'

# 数据定义
functions = {}
interface = {}

# 读取类型配置
cf.read(path_conf)
if 'type_func' in cf.sections():
	for item in cf.items('type_func'):
		functions[item[0]] = item[1]
if 'interface' in cf.sections():
	for item in cf.items('interface'):
		interface[item[0]] = item[1]
#print functions

# 名称操作
def to_title(src):
	return src.title().replace('_', '')

# 解析类型
def parse_type(variable):
	if type(variable) == types.IntType:     return 'int'
	if type(variable) == types.FloatType:   return 'float'
	if type(variable) == types.BooleanType: return 'bool'
	if type(variable) == types.UnicodeType: return 'string'
	if type(variable) == types.ListType:    return 'JsonArray'
	if type(variable) == OrderedDict:       return 'JsonObject'
	return 'object'

# 解析优先级
def cover_type(pre, new):
	if pre == 'object': return True
	if pre == 'int'   and new == 'float' : return True
	if pre == 'int'   and new == 'string': return True
	if pre == 'float' and new == 'string': return True
	if pre == 'bool'  and new == 'string': return True
	return False

def parse_field(src):
	# 读取释义
	field_desc = {}
	for root, dirs, files in os.walk(path_xlsx):
		for xlsx in files:
			if xlsx == src + '.xlsx':
				workbook = xlrd.open_workbook(os.path.join(root, xlsx))
				sheet = workbook.sheets()[0]
				for col in range(1, sheet.ncols):
					field_desc[sheet.cell(0, col).value] = sheet.cell(1, col).value.replace('\n', ' ')
				break
	#print field_desc

	# 解析Json中类型
	jsonfile = file(path_json + '/' + src + '.json')
	jsonobj = json.loads(jsonfile.read(), object_pairs_hook=OrderedDict)
	jsonfile.close()

	# 有序的字典结构
	field_type = OrderedDict()
	for obj in jsonobj.values():
		for key in obj.keys():
			typestr = parse_type(obj[key])
			if not key in field_type:
				field_type[key] = typestr
			elif cover_type(field_type[key], typestr):
				field_type[key] = typestr	
	#print field_type			 

	# 覆盖为配置类型
	if src in cf.sections():
		for item in cf.items(src):
			if item[0] in field_type.keys():
				field_type[item[0]] = item[1]
			else:
				print_red('[Warning] 多余的配置：' + item[0] + ' = ' + item[1])
	#print field_type

	return field_desc, field_type

# 复制模版
def generator(src, desc, fieldtype):
	template = file(path_temp).read()

	# 类名称
	template = template.replace('#MODEL_NAME#', to_title(src), 1)

	# 接口名称
	if src in interface:
		template = template.replace('#INTERFACE#', ', ' + interface[src], 1)
	else:
		template = template.replace('#INTERFACE#', '', 1)

	# 属性字段
	propertys = ''
	for field in fieldtype:
		if field in desc.keys():
			propertys += '	//' + desc[field] + '\n'
		else:
			propertys += '	//\n'
		propertys += '	public {0} {1}\n'.format(fieldtype[field], to_title(field))
		propertys += '	{\n        get;\n        private set;\n    }\n'
		if not field == fieldtype.keys()[len(fieldtype.keys()) - 1]:
			propertys += '\n'
	template = template.replace('#PROPERTIES#', propertys, 1)

	# 构造函数
	constructor = ''
	for field in fieldtype:
		if fieldtype[field].lower() in functions.keys():
			constructor += '		{0} = {1}(config, "{2}");'.format(to_title(field), functions[fieldtype[field].lower()], field)
		else:
			constructor += '		//{0}无法解析'.format(to_title(field))
			print_red('[Error] (' + fieldtype[field] + ')' + field + '无解析函数')
		if not field == fieldtype.keys()[len(fieldtype.keys()) - 1]:
			constructor += '\n'
	template = template.replace('#DESERIALIZE#', constructor, 1)
	#print template

	return template

# 写入文件
def write_file(filepath, content):
	csfile = codecs.open(filepath, 'w', 'utf_8_sig')
	csfile.write(content)
	csfile.close()
	print_green('[Output]' + os.path.realpath(filepath))

# Main
name_src = ''
if len(sys.argv) > 1:
	name_src = sys.argv[1]

if os.path.exists(os.path.join(path_json, name_src + '.json')):
	field_desc, field_type = parse_field(name_src)
	model_text = generator(name_src, field_desc, field_type)
	write_file(os.path.join(path_data, to_title(name_src) + '.cs'), model_text)
else:
	print_red('[Error] Not available json file')

if(platform.system() == 'Windows'):
    raw_input(unicode('按回车键退出...','utf-8').encode('gbk'))