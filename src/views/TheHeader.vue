<template>
    <div class="header-contain">
        <div class="header-left">
            <div class="dropdown-contain">
                <Combobox
                    type="cbxHead"
                    ref="inputBranch"
                    id="txtBranch"
                    fieldType="branch"
                    displayName="Đơn vị"
                    value=""
                    placeholder=""
                    itemId="BranchId"
                    itemName="BranchName"
                    tabindex=""
                />
                <!-- ref="textComboboxBranch" -->
            </div>
        </div>
        <div class="header-right">
            <div class="version">R58.0</div>
            <div class="language-contain">
                <div class="language-dropdown">
                    <Combobox
                        type="cbxHead"
                        ref="inputLanguage"
                        id="txtLanguage"
                        fieldType="language"
                        displayName="Đơn vị"
                        value=""
                        placeholder=""
                        :required="true"
                        itemId="LanguageId"
                        itemName="LanguageName"
                    />
                </div>
            </div>
            <div class="notification">
                <div class="notification-icon"></div>
            </div>
            <div class="inner-support"></div>
            <div class="user-container">
                <div class="avatar"></div>
                <div class="username">Nguyễn Thị Dung</div>
                <div class="user-contain-icon"></div>
            </div>
        </div>
    </div>
</template>

<script>
import Combobox from '../components/base/Combobox.vue';

export default {
    components:{
        Combobox,
    },
    data() {
        return {
            // data cho combobox
            // dataComboboxBranch: [
            //     { Text: "Chuỗi nhà hàng", Value: 1 },
            //     { Text: "Kho tổng", Value: 2 },
            //     { Text: "Nhà hàng Ánh Dương", Value: 3 },
            //     { Text: "Nhà hàng Buffet", Value: 4 },
            // ],

            // dataComboboxLanguage: [
            //     { Text: "Tiếng Việt", Value: 1 },
            //     { Text: "Tiếng Anh", Value: 2 },
            //     { Text: "Tiếng Trung", Value: 3 },
            //     { Text: "Tiếng Đức", Value: 4 },
            // ],
        }
    },
}
</script>

<style scoped>
@import url(../css/view/header.css);
</style>