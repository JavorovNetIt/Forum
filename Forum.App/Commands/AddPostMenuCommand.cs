using Forum.App.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.App.Commands
{
    public class AddPostMenuCommand : MenuCommand
    {
        public AddPostMenuCommand(IMenuFactory menuFactory) : base(menuFactory)
        {
        }
    }
}
