using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V0_2_PersistenceComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Courses_CourseId",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Unit_CourseId",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmissionAnswers",
                table: "SubmissionAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassroomStudents",
                table: "ClassroomStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classrooms",
                table: "Classrooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentQuestions",
                table: "AssignmentQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentClassrooms",
                table: "AssignmentClassrooms");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Submissions",
                newName: "Submission");

            migrationBuilder.RenameTable(
                name: "SubmissionAnswers",
                newName: "SubmissionAnswer");

            migrationBuilder.RenameTable(
                name: "QuestionOptions",
                newName: "QuestionOption");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameTable(
                name: "ClassroomStudents",
                newName: "ClassroomStudent");

            migrationBuilder.RenameTable(
                name: "Classrooms",
                newName: "Classroom");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "Assignment");

            migrationBuilder.RenameTable(
                name: "AssignmentQuestions",
                newName: "AssignmentQuestion");

            migrationBuilder.RenameTable(
                name: "AssignmentClassrooms",
                newName: "AssignmentClassroom");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "QuestionOption",
                newName: "DisplayOrder");

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "Question",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "SubmissionAnswer",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "SubmissionAnswer",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "QuestionOption",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classroom",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "JoinCode",
                table: "Classroom",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Assignment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submission",
                table: "Submission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmissionAnswer",
                table: "SubmissionAnswer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionOption",
                table: "QuestionOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassroomStudent",
                table: "ClassroomStudent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignment",
                table: "Assignment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentQuestion",
                table: "AssignmentQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentClassroom",
                table: "AssignmentClassroom",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ExternalIdentityId",
                table: "User",
                column: "ExternalIdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submission_AssignmentId_StudentId",
                table: "Submission",
                columns: new[] { "AssignmentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submission_StudentId",
                table: "Submission",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionAnswer_AssignmentQuestionId",
                table: "SubmissionAnswer",
                column: "AssignmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionAnswer_SubmissionId_AssignmentQuestionId",
                table: "SubmissionAnswer",
                columns: new[] { "SubmissionId", "AssignmentQuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOption_QuestionId_DisplayOrder",
                table: "QuestionOption",
                columns: new[] { "QuestionId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomStudent_ClassroomId_StudentId",
                table: "ClassroomStudent",
                columns: new[] { "ClassroomId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomStudent_StudentId",
                table: "ClassroomStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classroom_CourseId",
                table: "Classroom",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classroom_JoinCode",
                table: "Classroom",
                column: "JoinCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classroom_TeacherId",
                table: "Classroom",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_ClassroomId",
                table: "Assignment",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentQuestion_AssignmentId_Order",
                table: "AssignmentQuestion",
                columns: new[] { "AssignmentId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentQuestion_AssignmentId_QuestionId",
                table: "AssignmentQuestion",
                columns: new[] { "AssignmentId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentQuestion_QuestionId",
                table: "AssignmentQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentClassroom_AssignmentId_ClassroomId",
                table: "AssignmentClassroom",
                columns: new[] { "AssignmentId", "ClassroomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentClassroom_ClassroomId",
                table: "AssignmentClassroom",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Classroom_ClassroomId",
                table: "Assignment",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentClassroom_Assignment_AssignmentId",
                table: "AssignmentClassroom",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentClassroom_Classroom_ClassroomId",
                table: "AssignmentClassroom",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentQuestion_Assignment_AssignmentId",
                table: "AssignmentQuestion",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentQuestion_Question_QuestionId",
                table: "AssignmentQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classroom_Course_CourseId",
                table: "Classroom",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classroom_User_TeacherId",
                table: "Classroom",
                column: "TeacherId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassroomStudent_Classroom_ClassroomId",
                table: "ClassroomStudent",
                column: "ClassroomId",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassroomStudent_User_StudentId",
                table: "ClassroomStudent",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Assignment_AssignmentId",
                table: "Submission",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_User_StudentId",
                table: "Submission",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionAnswer_AssignmentQuestion_AssignmentQuestionId",
                table: "SubmissionAnswer",
                column: "AssignmentQuestionId",
                principalTable: "AssignmentQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionAnswer_Submission_SubmissionId",
                table: "SubmissionAnswer",
                column: "SubmissionId",
                principalTable: "Submission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Course_CourseId",
                table: "Unit",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Classroom_ClassroomId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentClassroom_Assignment_AssignmentId",
                table: "AssignmentClassroom");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentClassroom_Classroom_ClassroomId",
                table: "AssignmentClassroom");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentQuestion_Assignment_AssignmentId",
                table: "AssignmentQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentQuestion_Question_QuestionId",
                table: "AssignmentQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Classroom_Course_CourseId",
                table: "Classroom");

            migrationBuilder.DropForeignKey(
                name: "FK_Classroom_User_TeacherId",
                table: "Classroom");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassroomStudent_Classroom_ClassroomId",
                table: "ClassroomStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassroomStudent_User_StudentId",
                table: "ClassroomStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOption_Question_QuestionId",
                table: "QuestionOption");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Assignment_AssignmentId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_User_StudentId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionAnswer_AssignmentQuestion_AssignmentQuestionId",
                table: "SubmissionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionAnswer_Submission_SubmissionId",
                table: "SubmissionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Course_CourseId",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ExternalIdentityId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmissionAnswer",
                table: "SubmissionAnswer");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionAnswer_AssignmentQuestionId",
                table: "SubmissionAnswer");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionAnswer_SubmissionId_AssignmentQuestionId",
                table: "SubmissionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submission",
                table: "Submission");

            migrationBuilder.DropIndex(
                name: "IX_Submission_AssignmentId_StudentId",
                table: "Submission");

            migrationBuilder.DropIndex(
                name: "IX_Submission_StudentId",
                table: "Submission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionOption",
                table: "QuestionOption");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOption_QuestionId_DisplayOrder",
                table: "QuestionOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassroomStudent",
                table: "ClassroomStudent");

            migrationBuilder.DropIndex(
                name: "IX_ClassroomStudent_ClassroomId_StudentId",
                table: "ClassroomStudent");

            migrationBuilder.DropIndex(
                name: "IX_ClassroomStudent_StudentId",
                table: "ClassroomStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom");

            migrationBuilder.DropIndex(
                name: "IX_Classroom_CourseId",
                table: "Classroom");

            migrationBuilder.DropIndex(
                name: "IX_Classroom_JoinCode",
                table: "Classroom");

            migrationBuilder.DropIndex(
                name: "IX_Classroom_TeacherId",
                table: "Classroom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentQuestion",
                table: "AssignmentQuestion");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentQuestion_AssignmentId_Order",
                table: "AssignmentQuestion");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentQuestion_AssignmentId_QuestionId",
                table: "AssignmentQuestion");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentQuestion_QuestionId",
                table: "AssignmentQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssignmentClassroom",
                table: "AssignmentClassroom");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentClassroom_AssignmentId_ClassroomId",
                table: "AssignmentClassroom");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentClassroom_ClassroomId",
                table: "AssignmentClassroom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignment",
                table: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_ClassroomId",
                table: "Assignment");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "SubmissionAnswer",
                newName: "SubmissionAnswers");

            migrationBuilder.RenameTable(
                name: "Submission",
                newName: "Submissions");

            migrationBuilder.RenameTable(
                name: "QuestionOption",
                newName: "QuestionOptions");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "ClassroomStudent",
                newName: "ClassroomStudents");

            migrationBuilder.RenameTable(
                name: "Classroom",
                newName: "Classrooms");

            migrationBuilder.RenameTable(
                name: "AssignmentQuestion",
                newName: "AssignmentQuestions");

            migrationBuilder.RenameTable(
                name: "AssignmentClassroom",
                newName: "AssignmentClassrooms");

            migrationBuilder.RenameTable(
                name: "Assignment",
                newName: "Assignments");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "QuestionOptions",
                newName: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "Question",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(320)",
                oldMaxLength: 320);

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "SubmissionAnswers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "SubmissionAnswers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "QuestionOptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classrooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "JoinCode",
                table: "Classrooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmissionAnswers",
                table: "SubmissionAnswers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissions",
                table: "Submissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionOptions",
                table: "QuestionOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassroomStudents",
                table: "ClassroomStudents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classrooms",
                table: "Classrooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentQuestions",
                table: "AssignmentQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssignmentClassrooms",
                table: "AssignmentClassrooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_CourseId",
                table: "Unit",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Courses_CourseId",
                table: "Unit",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
