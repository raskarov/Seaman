(function () {
    angular.module("blocks.validation")
        .factory("validation", validationService);

    validationService.$inject = ["helper"];

    function validationService(helper) {
        var messages = {
            required: 'Введите {0}',
            requiredSelect: 'Выберите {0}',
            minLength: 'Длинна должна быть не менее {0} символов',
            maxLength: 'Длина должна быть не более {0} символов',
            date: 'Введите корректную дату',
            passwordVerify: 'Пароль и подтверждение пароля не совпадают',
            compareDate: "Даты не равны"
        };

        var service = {
            requiredMessage: getRequiredMessage,
            requiredSelectMessage: getRequiredSelectMessage,
            minLengthMessage: getMinLengthMessage,
            maxLengthMessage: getMaxLengthMessage,
            passwordVerifyMessage: getPasswordVerifyMessage,
            getDateMessage: getDateMessage,
            getErrors: getErrors,
            Field: Field,
            newField: newField
        };

        return service;

        function getRequiredMessage(fieldName) {
            return helper.format(messages.required, fieldName);
        };

        function getMinLengthMessage(minLength) {
            return helper.format(messages.minLength, minLength);
        };

        function getMaxLengthMessage(maxLength) {
            return helper.format(messages.maxLength, maxLength);
        };

        function getRequiredSelectMessage(fieldName) {
            return helper.format(messages.requiredSelect, fieldName);
        };

        function getDateMessage() {
            return messages.date;
        };

        function getErrors(form, fields) {
            if (!form || !fields) return [];
            var result = {};
            _.forEach(form.$error, function (error, key) {
                _.forEach(error, function (field, j) {
                    if (!result[field.$name]) {
                        result[field.$name] = {};
                        result[field.$name].name = fields[field.$name].title;
                        result[field.$name].errors = [];
                    }
                    result[field.$name].errors.push(fields[field.$name].message[key]);
                });
            });

            return result;
        };

        function getPasswordVerifyMessage() {
            return messages.passwordVerify;
        };

        function getCompareDate() {
            return messages.compareDate;
        }

        function newField(title, params) {
            return new Field(title, params);
        };

        function Field(title, params) {
            if (!title) return this;
            this.title = title;
            this.baseTitle = title;
            var message = this.message = {};
            if (!params) return this;
            if (params.required) {
                if (angular.isString(params.required)) {
                    message.required = getRequiredMessage(params.required);
                } else {
                    message.required = getRequiredMessage(this.title.toLowerCase());
                }
            }
            if (params.requiredSelect) {
                if (angular.isString(params.requiredSelect)) {
                    message.required = getRequiredSelectMessage(params.requiredSelect);
                } else {
                    message.required = getRequiredSelectMessage(this.title.toLowerCase());
                }
            }
            if (params.requiredSelect || params.required) {
                this.title = this.title + '*';
            }
            if (params.minLength) {
                this.minLength = params.minLength;
                message.minlength = getMinLengthMessage(params.minLength);
            }
            if (params.maxLength) {
                this.maxLength = params.maxLength;
                message.maxlength = getMaxLengthMessage(params.maxLength);
            }
            if (params.date) {
                message.date = getDateMessage();
            }
            if (params.passwordVerify) {
                message.passwordVerify = getPasswordVerifyMessage();
            }

            if (params.compareDate) {
                message.compareDate = angular.isString(params.compareDate) ? params.compareDate : getCompareDate();
            }
            return this;
        };
    };
})();
