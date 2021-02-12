using System;
using HotChocolate.Configuration;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppAny.HotChocolate.FluentValidation
{
	public static class RequestExecutorBuilderExtensions
	{
		/// <summary>
		/// Adds default validation services with <see cref="ValidationBuilder"/> overrides
		/// </summary>
		public static IRequestExecutorBuilder AddFluentValidation(
			this IRequestExecutorBuilder builder,
			Action<ValidationBuilder>? configure = null)
		{
			var validationOptions = new ValidationOptions();

			configure?.Invoke(new DefaultValidationBuilder(validationOptions));

			builder.SetContextData(ValidationDefaults.ValidationOptionsKey, validationOptions);

			builder.OnBeforeCompleteType(ValidationDefaults.Interceptors.OnBeforeCompleteType);

			builder.TryAddSchemaInterceptor(
				new DelegateSchemaInterceptor(
					onAfterCreate: ValidationDefaults.Interceptors.OnAfterCreate));

			builder.OnBeforeRegisterDependencies(
				ValidationDefaults.Interceptors.OnBeforeRegisterDependencies);

			return builder;
		}
	}
}
