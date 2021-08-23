--[[
-- added by Golden @ 2017-12-18
-- string扩展工具类，对string不支持的功能执行扩展
--]]

local NumberUtil= {}


-- 思路：首先为有效的数字，其次分离整数、小数部分，整数部分三位拼一个逗号，最后拼接小数部分。
-- 格式化数字 （每三位加逗号）
function NumberUtil.FormatNum3( numTmp )
    local resultNum = numTmp
    if type(numTmp) == "number" then
		numTmp = math.floor(numTmp)
        local inter, point = math.modf(numTmp)
        local strNum = tostring(inter)
        local newStr = ""
        local numLen = string.len( strNum )
        local count = 0
        for i = numLen, 1, -1 do
            if count % 3 == 0 and count ~= 0  then
                newStr = string.format("%s,%s",string.sub( strNum,i,i),newStr) 
            else
                newStr = string.format("%s%s",string.sub( strNum,i,i),newStr) 
            end
            count = count + 1
        end

        if point > 0 then
            --@desc 存在小数点，
            local strPoint = string.format( "%.1f", point )
            resultNum = string.format("%s%s",newStr,string.sub( strPoint,2, string.len( strPoint ))) 
        else
            resultNum = newStr
        end
    end
    return resultNum
end

-- 需求：
-- 1，金币个数小于5位数，全显示。
-- 2，金币个数 >= 5且 < 9，除以1w，保留2位小数后带有“万”单位。
-- 3，金币个>=9，除以1亿，保留2位小数后带有“亿”单位。
-- 4，带有“万”、“亿”并保留两位小数，但不做四舍五入处理。如：12345缩进万，显示1.23万。
-- 细节：若小数点最后位是0，则隐藏最后位的0；若小数点后两位为0，则隐藏小数点和两个0。

function NumberUtil.FormatNumberCN(number)
	if number == nil or type(number) ~= "number" then
	   log("参数数值类型错误")
	else
		local num_length, num_str = string.len(number), tostring(number)
		local function handler_dot(curNum, curUnit)
		local tmp_decimal_str = ""
		local integer_num, dot_num = math.modf(curNum/curUnit)
		local decimal_num = curNum/curUnit
		if decimal_num ~= 0 and string.find(tostring(decimal_num), "%.") then
		tmp_decimal_str = tmp_decimal_str .. string.sub(tostring(decimal_num), string.find(tostring(decimal_num), "%."), string.find(tostring(decimal_num), "%.") + 2)
		local first_str = string.sub(tostring(tmp_decimal_str), 2, 2)
		local second_str= string.sub(tostring(tmp_decimal_str), 3, 3)
				if second_str ~= "0" and second_str ~= "" then
					--tmp_decimal_str = tmp_decimal_str .. string.sub(tostring(decimal_num), 2, string.find(tostring(decimal_num), ".") + 3)
				else
				if first_str ~= "" and first_str ~= "0" then
						tmp_decimal_str = "." .. first_str
				else
				tmp_decimal_str = ""
				end
			end
		end	 
		return tostring(integer_num) .. tmp_decimal_str
		end
 
	  if num_length < 5 then
		return num_str
	  elseif num_length >= 5 and num_length < 9 then--万
		return handler_dot(number, 10^4) .. "万"
	  elseif num_length >= 9 then--亿
		return handler_dot(number, 10^8) .. "亿"
	  end
	 end
 end

function NumberUtil.FormatNumber9999(number)
	if number == nil or type(number) ~= "number" then
	   log("参数数值类型错误")
	else
		if(number > 9999)then
			return "9999+"
		else
			return tostring(number)
		end
	end
end

-- 将数字以，每三位隔开：
-- function toThousandslsFilter(num)
-- 	return (+num || 0).toString().replace(/^-?\d+/g, m => m.replace(/(?=(?!\b)(\d{3})+$)/g, ','));
-- end

-- 字格式化成K,M等格式
-- num 数字 123456
-- digits 保留几位小数
 function NumberUtil.FormatterKM(num)
	if num == nil or type(num) ~= "number" then
		log("参数数值类型错误")
		return num
	end
	-- 当数字小于等于9999时正常显示全部数字，例9999
	if(num <= 99999) then
		return tostring(num)
	end
	-- 当大于9999，小于等于9999999时，使用带有一位小数点的K为单位，例10.1K
	-- 当数字大于9999999时，使用带有一位小数点的M为单位，例10.1M
	local si = {
		{ value = 1, symbol = "" },
		{ value = 1E3, symbol = "K" },
		{ value = 1E6, symbol = "M" },
		{ value = 1E9, symbol = "G" },
		{ value = 1E12, symbol = "T" },
		{ value = 1E15, symbol = "P" },
		{ value = 1E18, symbol = "E" }
	}

	local ii;
	for i= 3, 1, -1 do
		if (num >= si[i].value) then
			ii = i
			break
		end
	end
	local tpnum = string.format("%.1f",num / si[ii].value)
	local t1, t2 = math.modf(tpnum)
	---小数如果为0，则去掉
	if t2 > 0 then
	else
		tpnum = t1
	end

	return tpnum..si[ii].symbol
end
-- string.format("%.1f",(value - 0.05)) --防止四舍五入，我们要向下取整
function NumberUtil.FormatPercent(num,onlyNumber)
	if math.floor(num)<num then
		num = string.format( "%.2f", num )
	end
	local t1, t2 = math.modf(num)
	---小数如果为0，则去掉
	if t2 > 0 then
		return onlyNumber == true and num or num.."%"
	else
		return onlyNumber == true and t1 or t1.."%"
	end
end

return NumberUtil