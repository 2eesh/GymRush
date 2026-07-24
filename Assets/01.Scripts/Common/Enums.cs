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

public enum EmployeeStatType
{
    Speed,    // 이동 속도 (MoveSpeed)
    WorkRate, // 처리 속도 (GuideGaugeRatePerSecond)
}