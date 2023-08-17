module Model.Employee

// Name is the employees name. Department is the department's identifier
// which always consists of four uppercase letters followed by two digits.
type Employee =
    { Name: string; DepartmentId: string }