<template>
    <div class="popup-mask" :class="{ isHidePopup : isHidePopup }">
        <div class="popup-contain">
            <div class="popup-top">
                <div class="popup-title">CUKCUK - Quản lý nhà hàng</div>
            </div>
            <div class="popup-bottom">
                <div class="popup-content">
                    <div v-if="this.modePopup === 0 || this.modePopup === 1" class="popup-icon popup-icon-question"></div>
                    <div v-if="this.modePopup === 2 || this.modePopup === 3" class="popup-icon popup-icon-warning"></div>
                    <div v-if="this.modePopup === 0" class="popup-text">Dữ liệu đã thay đổi, bạn có muốn cất không?</div>
                    <div v-else-if="this.modePopup === 1" class="popup-text">Bạn có chắc chắn muốn xóa Nguyên vật liệu &#60;&#60; {{ this.inventoryItemCode }} - {{ this.inventoryItemName }} &#62;&#62; không?</div>
                    <div v-else-if="this.modePopup === 2" class="popup-text">Đơn vị chuyển đổi không được trùng với đơn vị tính chính.</div>
                    <div v-else-if="this.modePopup === 3" class="popup-text">Mã &#60;{{ this.inventoryItemCode }}&#62; bị trùng với mã của NVL &#60;{{ this.inventoryItemName }}&#62; trong danh sách NVL.</div>
                </div>
                <div class="popup-button" >
                    <button v-if="this.modePopup === 0 || this.modePopup === 1" class="button button-popup" @click="acceptAction">
                        <div class="button-text button-popup-text">Có</div>
                    </button>

                    <button v-if="this.modePopup === 0 || this.modePopup === 1" class="button button-popup" @click="closePopup">
                        <div class="button-text button-popup-text">Không</div>
                    </button>

                    <button v-if="this.modePopup === 0" class="button button-popup" @click="closePopup">
                        <div class="button-text button-popup-text">Hủy bỏ</div>
                    </button>

                    <button v-if="this.modePopup === 2 || this.modePopup === 3" class="button button-popup" @click="closePopup">
                        <div class="button-text button-popup-text">Đồng ý</div>
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { POPUP_STATE, STATUS_CODE } from '../../resources/enum';
import { MESSAGE } from '../../resources/const';
import eventBus from '../../eventBus';
import { URL } from '../../resources/enum';
import axios from "axios";
export default {
    data() {
        return {
            modePopup: POPUP_STATE.UPDATE_DATA, //mode popup: 0 - update-data; 1 - delete; 2 - trùng unit; 3 - trùng code
            isHidePopup: true, // Ẩn hiện Popup
            inventoryItemId: "", // Id NVL truyền vào
            inventoryItemCode: "", // Code NVL
            inventoryItemName: "", // Tên NVL
        }
    },
    created() {
        /**---------------------
         * Mở popup xóa NVL
         * CreatedBy: LNT (13/10)
         */
        eventBus.$on("passObjectToDelete", (value) => {
            this.modePopup = POPUP_STATE.DELETE;
            this.inventoryItemId = value.InventoryItemId;
            this.inventoryItemName = value.InventoryItemName;
            this.inventoryItemCode = value.InventoryItemCode;
            this.isHidePopup = false;
            console.log(this.inventoryItemId);
        });
        /**----------------------------
         * Mở popup trùng đơn vị tính
         * CreatedBy: LNT (13/10)
         */
        eventBus.$on("DuplicatedUnitId", (value) =>{
            this.modePopup = value;
            this.isHidePopup = false;
        });
        /**-----------------------
         * Mở popup trùng mã NVL
         * CreatedBy: LNT (13/10)
         */
        eventBus.$on("DuplicatedCode", (value) =>{
            this.modePopup = POPUP_STATE.DUPLICATED_CODE;
            this.isHidePopup = false;
            this.inventoryItemName = value.InventoryItemName;
            this.inventoryItemCode = value.InventoryItemCode;
        });

    },
    methods: {
        /**-----------------------
         * Đóng Popup
         * CreatedBy: LNT (07/10)
         */
        closePopup(){
            this.isHidePopup = true;
        },
        /**----------------------
         * Xóa NVL
         * CreatedBy: LNT (07/10)
         */
        async acceptAction(){
            if (this.modePopup == POPUP_STATE.DELETE) {
                try {
                    await axios
                    .delete(`${URL}/${this.inventoryItemId}`)
                    .then((res) => {
                        if (res.data.StatusCode == STATUS_CODE.SUCCESS) {
                            this.$toast.success(MESSAGE.DELETE_MSG_SUCCESS);
                        }
                        else{
                            this.$toast.error(MESSAGE.ERROR_DELETE_NOSUCCES);
                        }
                        console.log(res);
                    })
                    .catch((res) => {
                        this.$toast.error(MESSAGE.ERROR_DELETE_NOSUCCES);
                        console.log(res);
                    });
                } catch (error) {
                    this.$toast.error(MESSAGE.EXCEPTION_MSG);
                    console.log(error);
                }
                this.inventoryItemName = "";
                this.inventoryItemCode = "";
                this.isHidePopup = true;
                eventBus.$emit("reloadData");
            }
        },
    },
}
</script>

<style scoped>
@import url(../../css/base/popup.css);

.isHidePopup{
    display: none;
}
</style>