using System.Text.Json.Serialization;
using FirstLight.Enum;

namespace FirstLight.Dtos.Subject;

public class TeacherDeleteFromSubjectDto
{
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SubjectRoleType SubjectRole { get; set; }
}