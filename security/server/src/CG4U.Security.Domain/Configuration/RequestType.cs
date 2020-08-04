using System.ComponentModel.DataAnnotations;

namespace CG4U.Security.Domain.Configuration
{
    public enum RequestType
    {
        [Display(Name = "SceneChange")]
        SceneChange = 1,

        [Display(Name = "Face")]
        Face = 2,

        [Display(Name = "Carplate")]
        Carplate = 3,

        [Display(Name = "ImageDescription")]
        ImageDescription = 4,

        [Display(Name = "PersonGroupCreate")]
        PersonGroupCreate = 5,

        [Display(Name = "PersonGroupTrain")]
        PersonGroupTrain = 6,

        [Display(Name = "PersonGroupPersonCreate")]
        PersonGroupPersonCreate = 7,

        [Display(Name = "PersonGroupPersonAddFace")]
        PersonGroupPersonAddFace = 8,

        [Display(Name = "PersonGroupTrainGetStatus")]
        PersonGroupTrainGetStatus = 9
    }
}
