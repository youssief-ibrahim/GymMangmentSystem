using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementSystemBLL.ViewModels.AnalyticsViewModels;
using GymMangmentBLL.ViewModels.AnalysisViewModel;

namespace GymMangmentBLL.Services.InterFaces
{
    public interface IAnalysisService
    {
        AnalysisViewModel GetAnalysisData();
    }
}
