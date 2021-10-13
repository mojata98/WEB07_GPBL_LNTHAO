<template>
    <div class="table-paging-holder">
        <div class="toolbar">
            <button class="button button-toolbar" id="addNewInventoryItem" @click="addObject">
                <div class="button-icon icon-add"></div>
                <div class="button-text">Thêm</div>
            </button>
            <button class="button button-toolbar" id="cloneInventoryItem">
                <div class="button-icon icon-clone"></div>
                <div class="button-text">Nhân bản</div>
            </button>
            <button class="button button-toolbar" id="editInventoryItem" @click="editObject">
                <div class="button-icon icon-edit"></div>
                <div class="button-text">Sửa</div>
            </button>
            <button class="button button-toolbar" id="deleteInventoryItem" @click="deleteObject">
                <div class="button-icon icon-delete"></div>
                <div class="button-text">Xóa</div>
            </button>
            <button class="button button-toolbar" id="refreshInventoryItem" @click="refresh">
                <div class="button-icon icon-refresh"></div>
                <div class="button-text">Nạp</div>
            </button>
        </div>
        <div class="table-contain">
            <Table
                ref="table"
                :headers="headers"
                :inventoryItems="inventoryItems"
                @passObject="receiveObject"
                @inputFilterByInventoryItemGroupName="FilterByInventoryItemGroupName"
                @inputFilterByProperty="FilterByProperty"
            />
        </div>
        <div class="pagin-contain">
            <div class="paging-left">
                <div class="first-page" @click="firstPage"></div>
                <div class="prev-page" @click="prevPage"></div>
                <div class="separate-line"></div>
                <div class="pagination">
                    <span>Trang</span>
                    <input 
                        type="text" 
                        class="input" 
                        style="width: 40px; margin-left: 8px; margin-right: 8px;" 
                        v-model="keySearchIndex"
                        @keydown.enter="getByFilter"
                        >
                    <span>trên {{this.totalPage}}</span>
                </div>
                <div class="separate-line"></div>
                <div class="next-page" @click="nextPage"></div>
                <div class="last-page" @click="lastPage"></div>
                <div class="separate-line"></div>
                <div class="reload" @click="refresh"></div>
                <div class="separate-line"></div>
                <div class="combobox-paging-contain">
                    <Dropdown
                        :data="dataPage"
                        ref="textDropdownPaging"
                        @setValueDefaultDropdown="setValueDefaultDropdown"
                        @passValue="getValueFromDropdown"
                    />
                </div>
            </div>
            <div class="paging-right">
                <div class="paging-text">Hiển thị {{ this.pageSize * (this.pageIndex - 1) + 1 }} - {{ this.endRecord }} trên {{this.totalRecord}} kết quả</div>
            </div>
        </div>
        <Loading :isLoading="isLoading" />
        <InventoryForm
        :hideForm="hideForm"
        @closeForm="closeForm"
        @reloadData="refresh"
        ref="modeForm"
        />
    </div>
    
</template>

<script>
let headers = [
//   { text: "Mã nguyên vật liệu", align: "left", width: "130px" },
  { text: "Tên nguyên vật liệu", align: "center", width: "150px" },
  { text: "Tính chất", align: "left", width: "160px" },
  { text: "Đơn vị tính", align: "left", width: "100px" },
  { text: "Nhóm nguyên vật liệu", align: "left", width: "130px" },
  { text: "Ghi chú", align: "left", width: "130px" },
  { text: "Ngừng theo dõi", align: "right", width: "130px" },
];
import axios from "axios";
import Dropdown from '../base/Dropdown.vue';
import Table from '../base/Table.vue';
import InventoryForm from '../layout/InventoryForm.vue';
import { URL } from '../../resources/enum';
import Loading from '../base/Loading.vue';
import eventBus from '../../eventBus';
import { STATUS_CODE, FORM_STATE } from '../../resources/enum';
import { MESSAGE } from '../../resources/const';
export default {
    components:{
        Dropdown,
        Table,
        InventoryForm,
        Loading,
    },
    data() {
        return {
            // data cho dropdown paging
            dataPage: [
                { Text: "25", Value: 25 },
                { Text: "50", Value: 50 },
                { Text: "100", Value: 100 },
            ],
            hideForm: true, // Tắt bật Form 
            headers: headers, // Tiêu đề bảng
            isLoading: false, // Ẩn loading
            isInventoryItemNull: true, // Đánh dấu có object nào đc chọn k
            modeForm: FORM_STATE.ADD, // Trạng thái Form
            condition: "", // Điều kiện Filter

            pageIndex: 1, // Số trang mặc định
            pageSize: 25, // Số bản ghi mặc định
            totalPage: 20, // Tổng số trang 
            totalRecord: 500, // Tổng số bản ghi
            keySearchIndex: 1, // Index Trang mặc định
            endRecord: 25, // Index bản ghi cuối cùng xuất hiện trong table

            inventoryItem: {}, // Object 1 NVL
            inventoryItems: [], // Mảng nhiều object NVL
            inventoryItemId: "", // Id NVL

            unitConverts: [], // Mảng nhiều DVCT
            unitConvert: {}, // Object 1 DVCT
        }
    },
    created() {
        /**------------------------
         * Load data ban đầu
         * CreatedBy: LNT (07/10)
         */
        this.getByFilter();
        /**-----------------------
         * Reload data sau khi xóa
         * CreatedBy: LNT (07/10)
         */
        eventBus.$on("reloadData", () =>{
            this.getByFilter();
        });
        /**-----------------------------------------------
         * Nhận sự kiện mở Form khi ấn double click 1 hàng
         * CreatedBY: LNT (08/10)
         */
        eventBus.$on("openForm", () => {
            this.hideForm = false;
        });
    
    },
    methods: {
        
        /**-----------------------
         * Đóng Form
         * CreatedBy: LNT (03/10)
         */
        closeForm(){
            this.hideForm = true;
        },
        /*-----------------------------------------------------------------
        *Lấy ra danh sách theo các tiêu chí và phân trang
        *CreateBy: LNT(17/08)
        */
        async getByFilter() {
            var self = this;
            self.isInventoryItemNull = true;
            if (self.keySearchIndex >= self.totalPage) {
                self.keySearchIndex = self.totalPage;
            } 
            else if (self.keySearchIndex < 0){
                self.keySearchIndex = 1;
            }
            self.pageIndex = self.keySearchIndex;
            self.isLoading = true;
            try {
                await axios
                .get(`${URL}/Filter?pageIndex=${this.pageIndex}&pageSize=${this.pageSize}&condition=${this.condition}`)
                    .then((res) => {
                        if (res.data.StatusCode == STATUS_CODE.SUCCESS) {
                            self.inventoryItems = res.data.Data;
                            // self.employees.forEach((employee) => {
                            //     employee.Option = false;
                            // });
                            self.totalRecord = res.data.TotalRecord;
                            self.totalPage = res.data.TotalPage;
                            setTimeout(() => (self.isLoading = false), 1000);
                        } else {
                            this.$toast.error(MESSAGE.LOAD_DATA_FAIL);
                        }
                        
                        console.log(res.data.Data);
                })
            .catch((error) => {
                this.$toast.error(MESSAGE.EXCEPTION_MSG);
                console.log(error);
            });
            } catch (error) {
                console.log(error);
            }
            // Tính lại số bản ghi hiển thị
            if (self.pageIndex < self.totalPage) {
                self.endRecord = self.pageSize * self.pageIndex;
            }
            else{
                self.endRecord = self.pageSize * (self.pageIndex - 1) + (self.totalRecord % self.pageSize);
            }
        },
        /**----------------------
         * Refresh data
         * CreatedBy: LNT (04/10)
         */
        refresh(){
            this.keySearchIndex = 1;
            this.pageIndex = 1;
            this.pageSize = 25;
            this.condition = "";
            this.$refs.textDropdownPaging.setTextDefault();
            this.$refs.table.resetCombobox();
            this.getByFilter();
        },
        /**------------------------
         * Chuyển về trang trước
         * CreatedBy: LNT (04/10)
         */
        prevPage(){
            this.pageIndex--;
            this.keySearchIndex = this.pageIndex;
            this.getByFilter();
        },
        /**------------------------
         * Chuyển tới trang tiếp theo
         * CreatedBy: LNT (04/10)
         */
        nextPage(){
            this.pageIndex++;
            this.keySearchIndex = this.pageIndex;
            this.getByFilter();
        },
        /**--------------------------
         * Chuyển về trang đầu tiên
         * CreatedBy: LNT (04/10)
         */
        firstPage(){
            this.pageIndex = 1;
            this.keySearchIndex = 1;
            this.getByFilter();
        },
        /**--------------------------
         * Chuyển tới trang cuối cùng
         * CreatedBy: LNT (04/10)
         */
        lastPage(){
            this.pageIndex = this.totalPage;
            this.keySearchIndex = this.pageIndex;
            this.getByFilter();
        },
        /**------------------------------------
         * Gán giá trị mặc định cho dropdown
         * CreatedBy: LNT (04/10) 
         */
        setValueDefaultDropdown(value){
            this.pageSize = value;
        },
        /**----------------------------------------------------
         * Nhận giá trị Filter khi lọc theo số trang Dropdown
         * CreatedBy: LNT (04/10)
         */
        getValueFromDropdown(value){
            this.pageSize = value;
            this.keySearchIndex = 1;
            this.getByFilter();
        },
        /**------------------------
         * Nhận ID hàng khi focus
         * CreatedBy: LNT (07/10)
         */
        receiveObject(value){
            this.inventoryItem = value;
            this.isInventoryItemNull = false;
            this.inventoryItemId = value.InventoryItemId;
        },
        /**----------------------
         * Mở Form khi ấn THÊM
         * CreatedBy: LNT (03/10)
         */
        addObject(){
            this.hideForm = false;
            this.modeForm = FORM_STATE.ADD;
            this.inventoryItem = {};
            this.$refs.modeForm.receiveData(this.modeForm, "");
        },
        /**------------------------
         * Hàm xóa NVL
         * CreatedBy: LNT (07/10)
         */
        deleteObject(){
            if (this.isInventoryItemNull == false){
                eventBus.$emit("passObjectToDelete", this.inventoryItem);
            }
            
            // console.log(this.inventoryItem);
        },
        /**------------------------
         * Sửa thông tin
         * CreatedBY: LNT (08/10)
         */
        editObject(){
            if (this.isInventoryItemNull == false) {
                this.hideForm = false;
                this.modeForm = FORM_STATE.EDIT;
                this.$refs.modeForm.receiveData(this.modeForm, this.inventoryItemId);
            }
        },
        /**------------------------------
         * Filter dữ liệu theo nhóm NVL
         * CreatedBy: LNT (11/10)
         */
        FilterByInventoryItemGroupName(value){
            if (this.condition != "") {
                this.condition += "AND%20" + "InventoryItemGroupName%3D%22" + `${value}` + "%22";
            } else {
                this.condition = "InventoryItemGroupName%3D%22" + `${value}` + "%22";
            }
            
            console.log(this.condition);
            this.getByFilter();
        },
        /**---------------------------
         * Filter data theo Tính chất
         * CreatedBy: LNT (11/10)
         */
        FilterByProperty(value){
            if (this.condition != "") {
                 this.condition += "AND%20" + "PropertyOfInventoryItem%3D%22" + `${value}` + "%22";
            } else {
                this.condition = "PropertyOfInventoryItem%3D%22" + `${value}` + "%22";
            }
            
            console.log(this.condition);
            this.getByFilter();
        },
        /**-----------------------------------
         * Reload data sau khi thêm bản ghi
         * CreatedBy: LNT (13/10)
         */
        reFilterData(){
            this.keySearchIndex = 1;
            this.pageSize = 25;
            this.condition = "";
            this.getByFilter();
        }
    },
    
}
</script>

<style scoped>
@import url(../../css/layout/inventoryList.css);

#addNewInventoryItem{
    width: 85px;
}

#cloneInventoryItem{
    width: 114px;
}

#editInventoryItem{
    width: 75px;
}

#deleteInventoryItem{
    width: 73px;
}

#refreshInventoryItem{
    width: 74px;
}
</style>