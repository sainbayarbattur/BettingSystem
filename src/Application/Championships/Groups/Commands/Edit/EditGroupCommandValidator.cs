﻿namespace BettingSystem.Application.Championships.Groups.Commands.Edit
{
    using Common;
    using FluentValidation;

    public class EditGroupCommandValidator : AbstractValidator<EditGroupCommand>
    {
        public EditGroupCommandValidator()
            => this.Include(new GroupCommandValidator<EditGroupCommand>());
    }
}
