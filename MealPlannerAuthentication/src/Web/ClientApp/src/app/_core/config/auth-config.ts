import { LogLevel, StsConfigHttpLoader } from 'angular-auth-oidc-client';
import { map } from 'rxjs';
import { AppSettingsService } from '../services/app-settings.service';

export const httpLoaderFactory = (appSettingsService: AppSettingsService) => {
    const config$ = appSettingsService.getSettings().pipe(
        map((settings) => {
            return {
                authority: settings.authority,
                clientId: settings.clientId,
                redirectUrl: `${window.location.origin}/callback`,
                postLogoutRedirectUri: window.location.origin,
                postLoginRoute: '/home',
                forbiddenRoute: '/forbidden',
                unauthorizedRoute: '/status/unauthorized',
                scope: settings.scope,
                responseType: 'code',
                silentRenew: true,
                useRefreshToken: true,
                logLevel: LogLevel.Warn,
                historyCleanupOff: true,
            };
        })
    );

    return new StsConfigHttpLoader(config$);
};
