---@class CS.Task : CS.System.Object
CS.Task = {}

---@property readonly CS.Task.TaskID : CS.System.Int32
CS.Task.TaskID = nil

---@property readonly CS.Task.State : CS.TASK_STATE
CS.Task.State = nil

---@property readonly CS.Task.Current : CS.System.Object
CS.Task.Current = nil

---@param carrier : CS.UnityEngine.MonoBehaviour
---@return CS.Task
function CS.Task(carrier)
end

function CS.Task:Start()
end

function CS.Task:Stop()
end

function CS.Task:Restart()
end

function CS.Task:Pause()
end

function CS.Task:Resume()
end

---@param action : CS.System.Delegate
---@param args : CS.System.Object[]
function CS.Task:ResetParam(action, args)
end

---@return CS.System.Boolean
function CS.Task:MoveNext()
end

function CS.Task:Reset()
end