module Department

/// A department has an Id (four uppercase letters followed by two digits),
/// a Name (only letters and spaces, but cannot contain two or more consecutive spaces),
/// and a list of subdepartments (which may be empty)
type Department = { Id: string;
                    Name: string;
                    Subdepartments: List<Department> }