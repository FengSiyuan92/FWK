--[[
-- added by Golden @ 2017-12-18
-- string扩展工具类，对string不支持的功能执行扩展
--]]

local unpack = unpack or table.unpack

-- 字符串分割
-- @split_string：被分割的字符串
-- @pattern：分隔符，可以为模式匹配
-- @init：起始位置
-- @plain：为true禁用pattern模式匹配；为false则开启模式匹配
local function split(split_string, pattern, search_pos_begin, plain)
	assert(type(split_string) == "string")
	assert(type(pattern) == "string" and #pattern > 0)
	search_pos_begin = search_pos_begin or 1
	plain = plain or true
	local split_result = {}

	while true do
		local find_pos_begin, find_pos_end = string.find(split_string, pattern, search_pos_begin, plain)
		if not find_pos_begin then
			break
		end
		local cur_str = ""
		if find_pos_begin > search_pos_begin then
			cur_str = string.sub(split_string, search_pos_begin, find_pos_begin - 1)
		end
		split_result[#split_result + 1] = cur_str
		search_pos_begin = find_pos_end + 1
	end

	if search_pos_begin <= string.len(split_string) then
		split_result[#split_result + 1] = string.sub(split_string, search_pos_begin)
	else
		split_result[#split_result + 1] = ""
	end

	return split_result
end

-- 字符串连接
function join(join_table, joiner)
	if #join_table == 0 then
		return ""
	end

	local fmt = "%s"
	for i = 2, #join_table do
		fmt = fmt .. joiner .. "%s"
	end

	return string.format(fmt, unpack(join_table))
end

function join2(join_table, pattern)
    if #join_table == 0 then
        return ""
    end
        local ret = ""
    local len = #list
    local str = ""
    for i = 1, len  do
        str = list[i]
        if i < len then
            ret = ret .. str .. pattern
        else
            ret = ret .. str
        end
    end
    return ret
end

-- 是否包含
-- target_string = "abcd1234" pattern = "cd"
-- 注意：plain为true时，关闭模式匹配机制，此时函数仅做直接的 “查找子串”的操作
function contains(target_string, pattern, plain)
	plain = plain or true
	local find_pos_begin, find_pos_end = string.find(target_string, pattern, 1, plain)
	return find_pos_begin ~= nil
end

-- 以某个字符串开始
-- target_string = "abcd1234" start_pattern = "abcd"
function startswith(target_string, start_pattern, plain)
	plain = plain or true
	local find_pos_begin, find_pos_end = string.find(target_string, start_pattern, 1, plain)
	return find_pos_begin == 1
end

-- 以某个字符串结尾
-- target_string = "abcd1234" start_pattern = "1234"
function endswith(target_string, start_pattern, plain)
	plain = plain or true
	local find_pos_begin, find_pos_end = string.find(target_string, start_pattern, -#start_pattern, plain)
	return find_pos_end == #target_string
end

string.formatError = "CSFormatError"

function formatCS(target_string,...)
	local success, res = pcall(CS.System.String.Format, target_string, ...)
	if not success then
		local s = tostring(target_string)
		local info = string.format("formatCS Error : fmt = %s, params = %s, stack = %s", s, table.dump({...}, true, 1), res)
		Logger.LogWarning(info)
	end
	return success and res or string.formatError
end

function trim(s)
	return (string.gsub(s, "^%s*(.-)%s*$", "%1"))
end
function trimAll(s)
	return string.gsub(s,"%s+","")
end

-- 字符串替换【不执行模式匹配】
-- s       源字符串
-- pattern 匹配字符串
-- repl    替换字符串
--
-- 成功返回替换后的字符串，失败返回源字符串
replace = function(s, pattern, repl)
	local i,j = string.find(s, pattern, 1, true)
	if i and j then
		local ret = {}
		local start = 1
		while i and j do
			table.insert(ret, string.sub(s, start, i - 1))
			table.insert(ret, repl)
			start = j + 1
			i,j = string.find(s, pattern, start, true)
		end
		table.insert(ret, string.sub(s, start))
		return table.concat(ret)
	end
	return s
end

local function getMin(a, b, c)
	local min = a
	if b<min then
		min = b
	end
	if c<min then
		min = c
	end
	return min
end


local function getDistance(s1, s2)
	-- 定义距离表
	local d = {}
	local l1 = utf8.len(s1)
	local l2 = utf8.len(s2)

	if l1 == 0 then return l2 end
	if l2 == 0 then return l1 end

	-- 二维数组第一行和第一列放置自然数
	for i = 1, l1 + 1 do
		d[i] = {}
		d[i][1] = i
	end
	for i = 1, l2 + 1 do
		d[1][i] = i
	end
	local c1, c2
	local s = utf8.sub
	-- 比较，若行列相同，则代价为0，否则代价为1；
	for i = 1, l1 do
		c1 = s(s1,i,i+1)
		for j = 1, l2 do
			c2 = s(s2,j,j+1)
			if c1 == c2 then
				d[i+1][j+1]=d[i][j]
			else
				d[i+1][j+1]=getMin(d[i][j+1],d[i+1][j],d[i][j]) + 1
			end
		end
	end
	return d[l1+1][l2+1], l1, l2
end

local function matchingRate(s1, s2)
	local d, l1, l2 = getDistance(s1, s2)
	local r = (1-d)/math.max(l1, l2)
	return r
end
-- 计算 UTF8 字符串的长度，每一个中文算一个字符
-- @function [parent=#string] utf8len
-- @param string input 输入字符串
-- @return integer#integer  长度
--[[--
计算 UTF8 字符串的长度，每一个中文算一个字符
~~~ lua
local input = "你好World"
log(string.utf8len(input))
-- 输出 7
~~~
]]
-- end --
local function utf8len(input)
    local len  = string.len(input)
    local left = len
    local cnt  = 0
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
    while left ~= 0 do
        local tmp = string.byte(input, -left)
        local i   = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

-- lzh
-- 功能：将字符串拆成单个字符，存在一个table中
-- eg. "有征收税租的权力，是对立有军功将领的奖励。"
-- return table = {{"char":"有","byteNum":3},{"char":"征","byteNum":3}} ...
local function utf8tochars(input)
     local list = {}
     local len  = string.len(input)
     local index = 1
     local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
     while index <= len do
        local c = string.byte(input, index)
        local offset = 1
        if c < 0xc0 then
            offset = 1
        elseif c < 0xe0 then
            offset = 2
        elseif c < 0xf0 then
            offset = 3
        elseif c < 0xf8 then
            offset = 4
        elseif c < 0xfc then
            offset = 5
        end
        local str = string.sub(input, index, index+offset-1)
        -- log(str)
        index = index + offset
        table.insert(list, {byteNum = offset, char = str})
     end

     return list
end
string.trim 		= trim -- 去空格
string.trimAll 		= trimAll -- 去空格all
string.split 		= split
string.join 		= join
string.contains 	= contains
string.startswith 	= startswith
string.endswith 	= endswith
string.formatCS 	= formatCS
string.replace		= replace
string.matchingRate = matchingRate
string.utf8len = utf8len
string.utf8tochars = utf8tochars
