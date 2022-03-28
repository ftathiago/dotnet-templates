namespace WebApi.DapperInfraData.Repositories
{
    public static class SampleRepositoryStmt
    {
        public const string Insert = @"
            insert into ""SampleTable"" (
                ""testProperty"", 
                active
            ) values(
                @TestProperty, 
                @Active
            )";

        public const string Update = @"
            update ""SampleTable"" set 
                ""testProperty"" = @TestProperty, 
                active = @Active 
            where id = @id";

        public const string Remove = @"
            delete from ""SampleTable"" 
            where id = @id";

        public const string GetById = @"
            select  id, 
                    ""testProperty"", 
                    active 
            from ""SampleTable"" 
            where id = @id";
    }
}
