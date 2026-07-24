public enum EquipmentState
{
    Empty,
    Using,
    Dirty,
}

public enum GuestState
{
    Enter,
    DecideNext,
    WaitInQueue,
    UseStation,
    DropMoney,
    Exit,
}

public enum GuestExpression
{
    Neutral,
    Happy,
    Annoyed,
}

public enum LockerState
{
    Empty,
    Using,
}

public enum CounterState
{
    Empty,
    Using,
}

public enum EmployeeState
{
    Idle,
    MoveToWork,
    Work,
}

public enum EmployeeRole
{
    CounterClerk,
    Cleaner,
}