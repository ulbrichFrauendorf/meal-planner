import { Injectable, NgModule } from '@angular/core';
import {
    AuthModule,
    LogLevel,
    OpenIdConfiguration,
    StsConfigLoader,
    StsConfigStaticLoader,
} from 'angular-auth-oidc-client';

@Injectable({ providedIn: 'root' })
export class ConfigService {
    getConfig(): OpenIdConfiguration {
        return {
            postLoginRoute: '/home',
            forbiddenRoute: '/forbidden',
            unauthorizedRoute: '/status/unauthorized',
            logLevel: LogLevel.Warn,
            historyCleanupOff: true,
            authority: 'https://localhost:5002',
            redirectUrl: `${window.location.origin}/callback`,
            postLogoutRedirectUri: window.location.origin,
            clientId: '351c102b-585e-493e-b457-b286f02e63ea',
            scope: 'openid profile mealplanner.api.scope offline_access',
            responseType: 'code',
            silentRenew: true,
            useRefreshToken: true,
        };
    }
}

const authFactory = (configService: ConfigService) => {
    const config = configService.getConfig();
    return new StsConfigStaticLoader(config);
};

@NgModule({
    imports: [
        AuthModule.forRoot({
            loader: {
                provide: StsConfigLoader,
                useFactory: authFactory,
                deps: [ConfigService],
            },
        }),
    ],

    exports: [AuthModule],
})
export class AuthConfigModule {}
