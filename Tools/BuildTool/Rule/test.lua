

function CheckTest(infoList)
	CLog.LogError("函数进入" .. infoList.Count)
	for i =0, infoList.Count -1, 1 do
		local ruleInfo = infoList[i]
		CLog.Log(ruleInfo.field.FieldName)
	end

	错错错
	local allType = Lookup.Datas:AllName()
	for	t = 0, 10000, 1 do
		for	i =0, allType.Length -1, 1 do
			local getter = Lookup.Datas[allType[i]]
			local alldata = getter:AllDatas()
			for j=0, alldata.Length -1, 1 do
				CLog.Log(alldata[j])
			end
		end
	end
end

-- 返回检查函数以及检查时机  0是编译完成时,1是数据结束后
return CheckTest, 0