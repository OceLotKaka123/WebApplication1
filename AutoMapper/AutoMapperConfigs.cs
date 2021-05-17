using BLL.Model;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    public class AutoMapperConfigs:Profile
    {
        public AutoMapperConfigs()
        {
            CreateMap<UserInfoModel, UserInfoEntity>();
            CreateMap<UserInfoEntity,CurrentUser>();
            CreateMap<UserInfoViewModel, CurrentUser>();
            CreateMap<CurrentUser, UserInfoViewModel>();
            CreateMap<CommentModel, SystemCommentEntity>();
            CreateMap<FundAnswerModel, FundAnswerEntity>();
        }
    }
}
