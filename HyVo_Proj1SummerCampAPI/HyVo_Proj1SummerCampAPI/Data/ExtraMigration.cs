using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampAPI.Data
{
    public static class ExtraMigration
    {
        public static void Steps(MigrationBuilder migrationBuilder)
        {
            //Camper
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER CampersTimestampOnUpdate
                    AFTER UPDATE ON Campers
                    BEGIN
                        UPDATE Campers
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetCampersTimestampOnInsert
                    AFTER INSERT ON Campers
                    BEGIN
                        UPDATE Campers
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");

            //Compound
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetCompoundsTimestampOnUpdate
                    AFTER UPDATE ON Compounds
                    BEGIN
                        UPDATE Compounds
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetCompoundsTimestampOnInsert
                    AFTER INSERT ON Compounds
                    BEGIN
                        UPDATE Compounds
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
        }
    }
}
