---@class CS.GameDrive : CS.UnityEngine.MonoBehaviour
CS.GameDrive = {}

---@field public CS.GameDrive.moduleName : CS.System.String[]
CS.GameDrive.moduleName = nil

---@field public CS.GameDrive.orders : CS.System.Int32[]
CS.GameDrive.orders = nil

---@property readwrite CS.GameDrive.Executable : CS.System.Boolean
CS.GameDrive.Executable = nil

---@return CS.GameDrive
function CS.GameDrive()
end

function CS.GameDrive:Pause()
end

function CS.GameDrive:Resume()
end