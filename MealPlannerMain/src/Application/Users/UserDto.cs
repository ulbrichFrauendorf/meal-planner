﻿using MealPlanner.Domain.Entities;

namespace MealPlanner.Application.Users;

public class UserDto
{
	public string Id { get; set; } = null!;
	public string Email { get; set; } = null!;

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<User, UserDto>();
		}
	}
}
