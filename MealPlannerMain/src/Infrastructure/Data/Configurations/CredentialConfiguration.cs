using System.Reflection.Emit;
using MealPlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MealPlanner.Infrastructure.Data.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
	public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
	{
		builder.HasOne(ri => ri.Recipe)
		   .WithMany(r => r.RecipeIngredients)
		   .HasForeignKey(ri => ri.RecipeId);

		builder.HasOne(ri => ri.Ingredient)
			.WithMany(i => i.RecipeIngredients)
			.HasForeignKey(ri => ri.IngredientId);
	}
}
