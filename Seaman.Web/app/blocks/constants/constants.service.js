﻿(function () {
    angular.module("blocks.constantsService")
        .constant('AUTH_EVENTS', {
            userReceived: 'user-received',
            loginSuccess: 'auth-login-success',
            loginFailed: 'auth-login-failed',
            logoutSuccess: 'auth-logout-success',
            sessionTimeout: 'auth-session-timeout',
            notAuthenticated: 'auth-not-authenticated',
            notAuthorized: 'auth-not-authorized',
            serverError: "server-error"
        })
        .constant('USER_ROLES', {
            admin: 'admin',
            embryologist: 'embryologist',
            reportGenerator: 'reportgenerator'
        })
        .constant("ROLE_TITLE", {
            admin: {
                name: "admin",
                title: 'Administrator'
            },
            embryologist: {
                name: 'embryologist',
                title: 'Embryologist'
            },
            reportGenerator: {
                name: 'reportgenerator',
                title: 'Report Generator'
            }
        })
        .constant("COMMON", {
            packageState: ".package",
            documentState: ".document",
            alphabet: "abcdefghijklmnopqrstuvwxyz"
        })
        .constant("COLORS", {
            white: "White",
            blue: "Blue",
            green: "Green",
            black: "Black",
            red: "Red",
            yellow: "Yellow",
            purple: "Purple",
            orange: "Orange",
            pink: "Pink"
        });
})();