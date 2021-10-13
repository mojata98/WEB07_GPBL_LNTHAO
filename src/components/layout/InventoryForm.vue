<template>
  <div
    class="form-mask"
    :class="{ hideForm: hideForm }"
    ref="inventoryItemForm"
  >
    <div class="form-contain">
      <div class="form-content">
        <div class="form-header">
          <div class="form-title">Thêm nguyên vật liệu</div>
          <div class="form-resize" @click="resizeForm"></div>
          <div class="form-exit" @click="closeForm"></div>
        </div>

        <ValidationObserver ref="modal_form">
        <div class="form-body">
          <div class="body-top">
            <div class="form-body-top">
              <div class="top-left">
                <div class="field">
                  <div class="field-title">
                    Tên&nbsp;<span style="color: red">(*)</span>
                  </div>
                  <div class="field-input" style="display: flex; width: 202px;align-items:center;">
                    <ValidationProvider
                      rules="required"
                      name="Tên NVL"
                      v-slot="{ errors }"
                      style="display: flex; align-items: center;"
                    >
                    <input
                      type="text"
                      class="input"
                      style="outline: none; width: 100%"
                      tabindex="1"
                      ref="InventoryItemName"
                      v-model="inventoryItem.InventoryItemName"
                      @blur="getNewCode"
                      :title="errors[0]"
                    />
                    <div 
                      style="align-items: center; display: flex;" 
                      :class="{'icon-warning': errors.length > 0 ? true : false}"
                      :title="errors[0]"  
                    ></div>
                    </ValidationProvider>
                  </div>
                </div>
                <div class="field">
                  <div class="field-title">
                    ĐVT&nbsp;<span style="color: red">(*)</span>
                  </div>
                  <div class="field-input" >
                    <!-- <input type="text" class="input" style="outline: none; width: 82px;"> -->
                    <ValidationProvider
                      rules="required"
                      name="Đơn vị tính"
                      v-slot="{ errors }"
                      style="display: flex;align-items:center;"
                    >
                    <Combobox
                      style="height: 25px; width: 104px"
                      type="cbxform"
                      ref="inputUnit"
                      id="txtUnit"
                      fieldType="unit"
                      displayName="Đơn vị tính"
                      v-model="inventoryItem.UnitId"
                      itemId="UnitId"
                      itemName="UnitName"
                      :required="true"
                      tabindex="3"
                      :selectedId="inventoryItem.UnitId"
                      :data="dataUnit"
                      @error="arrayError"
                      :title="errors[0]"
                    />
                    <div
                      class="icon-warning"
                      :class="{ 'hide-form-error': !(isError[1] == true) }"
                      :title="errors[0]"
                    >
                    <div class="sprite icon-form-invalid"></div>
                    </div>
                    </ValidationProvider>
                    <!-- @btnShowFormAdd="btnShowFormAdd" -->
                  </div>
                </div>
                <div class="field">
                  <div class="field-title">Hạn sử dụng</div>
                  <div
                    class="field-input"
                    style="display: flex; align-item: center; margin-top: 3px"
                  >
                    <money
                      type="text"
                      class="input"
                      style="outline: none; width: 92px"
                      tabindex="5"
                      ref="ExpirationNumber"
                      v-model="inventoryItem.ExpirationNumber"
                      v-bind="money"
                    />
                    <!-- <input type="text" class="input" style="outline: none; width: 82px; margin-left: 6px;"> -->
                    <Combobox
                      style="width: 92px; margin-left: 6px"
                      type="combo"
                      ref="inputExpriryType"
                      id="txtExpriryType"
                      fieldType="expriryType"
                      displayName="Kiểu hạn sử dụng"
                      v-model="inventoryItem.ExpirationType"
                      :value="formatExpirationType(inventoryItem.ExpirationType)"
                      :selectedId="formatExpirationType(inventoryItem.ExpirationType)"
                      itemId="ExpriryType"
                      itemName="ExpriryTypeName"
                      tabindex="6"
                    />
                    <!-- style="margin-left: 6px; width: 92px;display: none;"
                                            :data="dataCombobox"
                                            ref="textDropdownExp" -->
                  </div>
                </div>
              </div>
              <div class="top-right">
                <div class="field">
                  <div class="field-title">
                    Mã&nbsp;<span style="color: red">(*)</span>
                  </div>
                  <div class="field-input" style="display: flex; width: 202px;align-items:center;">
                    <ValidationProvider
                      rules="required"
                      name="Mã nguyên vật liệu"
                      v-slot="{ errors }"
                      style="display: flex; align-items: center;"
                    >
                    <input
                      type="text"
                      class="input"
                      style="outline: none; width: 100%"
                      tabindex="2"
                      ref="InventoryItemCode"
                      v-model="inventoryItem.InventoryItemCode"
                      
                    />
                    <div 
                      style="align-items: center; display: flex;" 
                      :class="{'icon-warning': errors.length > 0 ? true : false}"
                      :title="errors[0]"  
                    ></div>
                    </ValidationProvider>
                  </div>
                </div>
                <div class="field">
                  <div class="field-title">Kho ngầm định</div>
                  <div class="field-input">
                    <!-- <input type="text" class="input" style="outline: none; width: 190px;"> -->
                    <Combobox
                      style="width: 202px"
                      type="cbxform"
                      ref="inputStock"
                      id="txtStock"
                      fieldType="stock"
                      displayName="Kho ngầm định"
                      :selectedId="inventoryItem.StockId"
                      :value="inventoryItem.StockId"
                      v-model="inventoryItem.StockId"
                      itemId="StockId"
                      itemName="StockName"
                      tabindex="4"
                      :data="dataStock"
                    />
                    <!-- @btnShowFormAdd="btnShowFormAdd" -->
                  </div>
                </div>
                <div class="field">
                  <div class="field-title">SL tồn tối thiểu</div>
                  <div
                    class="field-input"
                    style="display: flex; align-item: center"
                  >
                    <money
                      type="text"
                      class="input"
                      style="outline: none; width: 82px"
                      tabindex="6"
                      v-model="inventoryItem.Quantity"
                      v-bind="money"
                    >
                    </money>
                  </div>
                </div>
              </div>
            </div>
            <div class="form-body-bottom">
              <div class="field-title">Mô tả</div>
              <div class="field-input">
                <textarea
                  name="description"
                  id="description"
                  cols="50"
                  rows="3"
                  style="outline: none"
                  tabindex="7"
                  v-model="inventoryItem.Description"
                ></textarea>
              </div>
            </div>
          </div>
          <div class="body-bottom">
            <div class="body-title">
              <div class="title-table">Đơn vị chuyển đổi</div>
            </div>
            <div class="body-table-contain">
              <table>
                <thead>
                  <tr>
                    <th style="width: 50px;">STT</th>
                    <th style="width: 150px;">Đơn vị chuyển đổi</th>
                    <th style="width: 230px;">Tỷ lệ chuyển đổi</th>
                    <th style="width: 100px;">Phép tính</th>
                    <th class="column-last" style="background-color: #EDEDED">Mô tả</th>
                  </tr>
                </thead>
                <tbody>
                  <tr
                    v-for="(record, index) in tableUnitConvert.data"
                    :key="'record' + index"
                    style="background-color: #fff; border-top: none; border-bottom: none;"
                  >
                    <td style="text-align: center; border-right: none;">
                      {{index + 1}}
                    </td>
                    <td style="padding: 0; border-bottom: block; border-right: none;">
                      <div>
                      <Combobox
                        type="cbxform"
                        ref="inputUnitConvert"
                        id="txtUnit"
                        fieldType="unitConvert"
                        displayName="Đơn vị tính"
                        itemId="UnitId"
                        itemName="UnitName"
                        :tabindex="`${index + 9}`"
                        styleList="max-height: 98px; overflow: auto;"
                        :data="dataUnit"
                        :selectedId="record['UnitId']"
                        v-model="record.UnitId"
                      />
                      </div>
                      <!-- :value="record['UnitId']" -->
                    </td>
                    <td style="padding: 0;">
                      <money 
                        type="text" 
                        class="input" 
                        v-model="record['CalculationRate']" 
                        style="width: calc(100% - 13px); text-align: right; padding-right: 7px;" 
                        v-bind="money"
                        :tabindex="`${index + 10}`"
                      > 
                      </money>
                    </td>
                    <td style="padding: 0; border-right: none;">
                      <Combobox
                        type="combo"
                        ref="inputConvertRateOperate"
                        id="txtConvertRateOperate"
                        fieldType="calculation"
                        displayName="Phép tính"
                        :selectedId="formatCalculation(record['Calculation'])"
                        itemId="Calculation"
                        itemName="CalculationName"
                        v-model="record['Calculation']"
                        :tabindex="`${index + 11}`"

                      />
                        <!-- value="formatCalculation(record['Calculation'])" -->

                    </td>
                    <td style="background-color: #EDEDED; width: 180px; white-space: nowrap;overflow: hidden;text-overflow: ellipsis;">
                      <!-- <span>{{ this.description }}</span> -->
                      <div></div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="body-button">
              <button class="button" id="addRow" @click="addNewRow">
                <div class="button-icon icon-add"></div>
                <div class="button-text">Thêm dòng</div>
              </button>
              <button class="button " id="deleteRow"
                :class="{'button-disable':this.tableUnitConvert.data.length > 0 ? false : true}"
              >
                <div class="button-icon icon-delete-row"></div>
                <div class="button-text">Xóa dòng</div>
              </button>
            </div>
          </div>
        </div>
        </ValidationObserver>

        <div class="form-footer">
          <div class="footer-left">
            <button class="button" id="help">
              <div class="button-icon form-icon-help"></div>
              <div class="button-text">Giúp</div>
            </button>
          </div>
          <div class="footer-right">
            <button class="button" id="save" @click="btnSave">
              <div class="button-icon form-icon-save"></div>
              <div class="button-text" >Cất</div>
            </button>
            <button class="button" id="saveAs" @click="saveAndCreate">
              <div class="button-icon form-icon-save-as"></div>
              <div class="button-text">Cất &amp; Thêm</div>
            </button>
            <button class="button" id="cancel" @click="closeForm">
              <div class="button-icon form-icon-cancel"></div>
              <div class="button-text">Hủy bỏ</div>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Combobox from "../base/Combobox.vue";
import { FORM_STATE, POPUP_STATE, STATUS_CODE, URL, URL_Stock, URL_Unit, URL_UnitConvert } from "../../resources/enum";
import { MESSAGE } from '../../resources/const';
import eventBus from '../../eventBus';
import axios from "axios";
import { Money } from "v-money";
import { ValidationObserver, ValidationProvider, extend } from "vee-validate";

extend("required", {
  validate(value) {
    return {
      required: true,
      valid: ["", null, undefined].indexOf(value) === -1,
    };
  },
  message: "Thông tin này là bắt buộc",
  computesRequired: true,
});

export default {
  components: {
    Combobox,
    Money,
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      //Data cho Combobox Hạn sử dụng
      dataCombobox: [
        { Text: "Ngày", Value: 1 },
        { Text: "Tháng", Value: 2 },
        { Text: "Năm", Value: 3 },
      ],

      inventoryItem: {}, // Object 1 NVL
      inventoryItems: [], // Mảng nhiều object NVL
      inventoryItemId: "", // Id NVL

      modeForm: FORM_STATE.ADD, // Trạng thái Form: 0 - ADD, 1 - EDIT

      countRows: 0, // Số lượng dòng trong bảng

      UnitConverts: [], // Mảng đơn vị chuyển đổi
      tableUnitConvert:{ // Bảng đơn vị chuyển đổi
        data: [],
      },
      description: "abc", // Nội dung mô tả trong bảng

      isChecked: [], // Lưu trạng thái dòng đang focus hay không

      dataStock: [], // Lưu dữ liệu thông tin kho

      dataUnit: [], // Lưu dữ liệu thông tin đơn vị tính

      money: { // Validate cho input dạng số
        decimal: ",",
        thousands: ".",
        precision: 0,
        masked: false,
      },

      isError: [false, false, false], // Check validate

      newCode: "", // Mã mới

    };
  },
  props: {
    hideForm: {
      type: Boolean,
      default: true,
    },
  },
  created() {
    this.loadStock();
    this.loadUnit();
    eventBus.$on("edit", (value) =>{
      this.modeForm = FORM_STATE.EDIT;
      this.inventoryItemId = value;
      this.bindDataToForm();
    });
  },
  methods: {

    // ------------------------- HÀM XỬ LÝ BUTTON --------------------------------//

    /**----------------------------
     * Nhận sự kiện khi ấn CẤT
     * CreatedBy: LNT (12/10)
     */
    btnSave(){
      this.$refs.modal_form.validate().then(async (success) => {
        if (!success) {
          // self.$nextTick(() => self.$refs.InventoryItemName.focus());
          return;
        }
        if (this.tableUnitConvert.data.length > 0){
          this.dataTableProcessing();
        }
        // console.log(this.inventoryItem);
        //debugger; //eslint-disable-line
        if (!this.isDuplicatedCode()) {
          if (this.modeForm == FORM_STATE.ADD) {
            await this.addNewInventoryItem();
            this.$emit("closeForm");
            this.$emit("reloadData"); 
            this.resetForm();
          }
          else if (this.modeForm == FORM_STATE.EDIT) {
            await this.editInventoryItem();
            this.resetForm();
            this.$emit("closeForm");
            this.$emit("reloadData");
          }
        }
      });
    },
    /**-----------------------------
     * Sự kiện nhấn nút CẤT VÀ THÊM
     * CreatedBy: LNT (13/10)
     */
    saveAndCreate(){
      this.resetForm();
    },
    /**-------------------------------------
     * Đóng Form khi ấn nút EXIT hoặc HỦY
     * CreatedBy: LNT (08/10)
     */
    closeForm() {
      this.$emit("closeForm");
      this.tableUnitConvert.data.length = 0;
      this.$refs.modal_form.reset();
      // this.$refs.inputUnitConvert.reset();
      this.isError[1] = false;
      this.$refs.inputUnit.setTextDefault();
      this.$refs.inputStock.setTextDefault();
      self.tableUnitConvert.data.length = 0;
    },
    /**
     * TEST
     */
    async resizeForm(){
      //debugger; //eslint-disable-line
      this.isDuplicatedCode();
    },
    /**---------------------------
     * Convert UTF8 to ASCII
     * CreatedBy: LNT (06/10)
     */
    removeVietnameseTones(str)
    {
        str = str.replace(/ à | á | ạ | ả | ã | â | ầ | ấ | ậ | ẩ | ẫ | ă | ằ | ắ | ặ | ẳ | ẵ /g, "a");
        str = str.replace(/ è | é | ẹ | ẻ | ẽ | ê | ề | ế | ệ | ể | ễ /g, "e");
        str = str.replace(/ ì | í | ị | ỉ | ĩ /g, "i");
        str = str.replace(/ ò | ó | ọ | ỏ | õ | ô | ồ | ố | ộ | ổ | ỗ | ơ | ờ | ớ | ợ | ở | ỡ /g, "o");
        str = str.replace(/ ù | ú | ụ | ủ | ũ | ư | ừ | ứ | ự | ử | ữ /g, "u");
        str = str.replace(/ ỳ | ý | ỵ | ỷ | ỹ /g, "y");
        str = str.replace(/ đ /g, "d");
        str = str.replace(/ À | Á | Ạ | Ả | Ã | Â | Ầ | Ấ | Ậ | Ẩ | Ẫ | Ă | Ằ | Ắ | Ặ | Ẳ | Ẵ /g, "A");
        str = str.replace(/ È | É | Ẹ | Ẻ | Ẽ | Ê | Ề | Ế | Ệ | Ể | Ễ /g, "E");
        str = str.replace(/ Ì | Í | Ị | Ỉ | Ĩ /g, "I");
        str = str.replace(/ Ò | Ó | Ọ | Ỏ | Õ | Ô | Ồ | Ố | Ộ | Ổ | Ỗ | Ơ | Ờ | Ớ | Ợ | Ở | Ỡ /g, "O");
        str = str.replace(/ Ù | Ú | Ụ | Ủ | Ũ | Ư | Ừ | Ứ | Ự | Ử | Ữ /g, "U");
        str = str.replace(/ Ỳ | Ý | Ỵ | Ỷ | Ỹ /g, "Y");
        str = str.replace(/ Đ /g, "D");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
        str = str.replace(/ \u02C6 |\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
        return str;
    },
    /**-----------------------
     * Xóa dấu trong chữ
     * CreatedBy: LNT (13/10)
     */
    removeAccents(str) {
      return str
        .normalize("NFD")
        .replace(/[\u0300-\u036f]/g, "")
        .replace(/đ/g, "d")
        .replace(/Đ/g, "D");
    },
    /**--------------------------------
     * Hàm nhận data và modeForm từ List
     * CreatedBy: LNT (08/10)
     */
    receiveData(formState, id) {
      this.modeForm = formState;
      this.inventoryItemId = id;
      if (this.modeForm == FORM_STATE.ADD) {
        this.resetForm();
        this.$nextTick(() => this.$refs.InventoryItemName.focus());
      }
      else{
        this.bindDataToForm();
      }
    },
    
    /**-------------------------
     * Thêm dòng mới trong bảng
     * CreatedBy: LNT (11/10)
     */
    addNewRow(){
      this.countRows = this.countRows + 1;
      let newRow = {
        Number: this.countRows,
        UnitId: "",
        CalculationRate: 1,
        Calculation: 0,
        Description: "",
      };
      this.tableUnitConvert.data.push(newRow);
      this.isChecked = new Array(this.tableUnitConvert.data.length).fill(false);
    },

    // ------------------------- HÀM LOAD DỮ LIỆU VÀO INPUT -------------------//

    /**------------------------
     * Lấy dữ liệu kho ngầm định
     * CreatedBy: LNT (05/10)
     */
    async loadStock() {
      try {
        await axios
          .get(URL_Stock)
          .then((res) => {
            this.dataStock = res.data.Data;
          })
          .catch((error) => {
            console.log(error);
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**----------------------------
     * Lấy dữ liệu cho đơn vị tính
     * CreatedBy: LNT (05/10)
     */
    async loadUnit() {
      try {
        await axios
          .get(URL_Unit)
          .then((res) => {
            this.dataUnit = res.data.Data;
          })
          .catch((error) => {
            console.log(error);
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**------------------------------
     * Sinh mã mới khi blur qua input ?? Check lại
     * CreatedBy: LNT (13/10)
     */
    async getNewCode(){
      let self = this;
      var value = self.inventoryItem.InventoryItemName;
      if (value) {
        value = this.removeAccents(value);
        const code = await self.autoNewCode(value);
        self.$set(self.inventoryItem, "InventoryItemCode",code);
      }
    },
    /**------------------------
     * Hàm reset form
     * CreatedBy: LNT (13/10)
     */
    resetForm(){
      this.inventoryItem = {};
      this.tableUnitConvert.data.length = 0;
      this.$refs.modal_form.reset();
      this.isError[1] = false;
      this.$refs.inputUnit.setTextDefault();
      this.$refs.inputStock.setTextDefault();
      this.tableUnitConvert.data.length = 0;
    },

    // ------------------------- HÀM XỬ LÝ DỮ LIỆU CỦA INPUT || VALIDATE -------------------//

    /**-----------------------
     * Format ngày tháng
     * CreatedBy: LNT (27/8)
     */
    formatExpirationType(value){
      if (value == 0) {
        return "0";
      }
      else if (value == 1){
        return "1";
      }
      else{
        return "2";
      }
    },
    /**--------------------------
     * Hàm chuyển đổi phép tính
     * CreatedBy: LNT (12/10)
     */
    formatCalculation(value){
      if (value == 0) {
        return "*";
      } else {
        return "/";
      }
    },
    /**------------------------
     * Validate combobox ??
     * CreatedBy: LNT (13/10)
     */
    arrayError(isShow, value){
      if (isShow == true) {
        this.$set(this.isError, value, true);
      } else {
        this.$set(this.isError, value, false);
      }
    },
    /**-----------------------------------------
     * Đóng gói dữ liệu bảng vào InventoryItem
     * CreatedBy: LNT (13/10)
     */
    dataTableProcessing(){
      // Thực hiện gán dữ liệu bảng vào mảng UnitConverts trong InventoryItem
      let unitConverts= [];
      console.log(this.tableUnitConvert.data);
      this.tableUnitConvert.data.forEach(item => {
        unitConverts.push(item);
      });
      this.inventoryItem.UnitConverts = unitConverts;

      // Convert string sang int các input chứa số
      var item = this.inventoryItem.UnitConverts;
      for (let index = 0; index < item.length; index++) {
        if (item[index].Calculation == "*") {
          item[index].Calculation = 0;
        }
        else if (item[index].Calculation == "/"){
          item[index].Calculation = 1;
        }
      }

      this.isDuplicatedUnitId();
    },
    /**--------------------------------------------
     * Check trùng UnitId của bảng vs UnitId chính: Trả về false - Không trùng mã; Trả về true: trùng mã
     * CreatedBy: LNT (13/10)
     */
    isDuplicatedUnitId(){
      var result = false;
      var array = this.inventoryItem.UnitConverts;
      for (let index = 0; index < array.length; index++) {
        if (array[index].UnitId == this.inventoryItem.UnitId) {
          eventBus.$emit("DuplicatedUnitId", POPUP_STATE.DUPLICATED_UNIT);
          result = true;
          return result;
        }
      }
      return result;
    },
    /**---------------
     * Check trùng mã : Trả về false - Không trùng mã; Trả về true: trùng mã
     * CreatedBy: LNT (13/10)
     */
    async isDuplicatedCode(){
      var result = false;
      const res = await this.getAllData();
      this.inventoryItems = res;
      this.inventoryItems.forEach(item => {
        if (
          this.inventoryItem.InventoryItemCode == item.InventoryItemCode &&
          this.inventoryItem.InventoryItemId != item.InventoryItemId
        ) {
          eventBus.$emit("DuplicatedCode", item);
          result = true;
        }           
      });
      return result;
    },

    // ---------------------- HÀM XỬ LÝ LIÊN QUAN API -----------------------//

    /**-----------------------
     * Hàm bind data lên Form
     * CreatedBy: LNT (08/10)
     */
    async bindDataToForm() {
      var self = this;
      // call api
      try {
        await axios
          .get(URL + `/${self.inventoryItemId}`)
          .then((res) => {
            if (res.data.StatusCode == STATUS_CODE.SUCCESS) {
              self.inventoryItem = res.data.Data;
              console.log(res);
              // Object.assign(this.employeeOriginalEdit, this.employee);
            } else {
              this.$toast.error(MESSAGE.EXCEPTION_MSG);
            }
          })
          .catch((error) => {
            console.log(error);
          });

        await axios
          .get(URL_UnitConvert + `/${self.inventoryItemId}`)
          .then((res) => {
            if (res.data.StatusCode == STATUS_CODE.SUCCESS) {
              self.UnitConverts = res.data.Data;
              self.tableUnitConvert.data = self.UnitConverts;
              console.log(this.tableUnitConvert.data);
              // Object.assign(this.employeeOriginalEdit, this.employee);
            } else {
              this.$toast.error(MESSAGE.EXCEPTION_MSG);
            }
          })
          .catch((error) => {
            console.log(error);
          }); 

          // self.loadUnit();
      } catch (error) {
        console.log(error);
      }
      self.$nextTick(() => self.$refs.InventoryItemName.focus());
    },
    /**------------------------
     * Thêm mới 1 NVL
     * CreatedBy: LNT (12/10)
     */
    async addNewInventoryItem(){
      var self = this;
      try {
        await axios
          .post(URL, self.inventoryItem)
          .then((res) => {
            if (res.data.StatusCode == STATUS_CODE.SUCCESS) {
              // self.inventoryItem = res.data.Data;
              self.$toast.success(MESSAGE.ADD_MSG_SUCCESS);
              console.log(res);
              // Object.assign(this.employeeOriginalEdit, this.employee);
            } else {
              this.$toast.error(MESSAGE.EXCEPTION_MSG);
            }
          })
          .catch((error) => {
            console.log(error);
            this.$toast.error(MESSAGE.EXCEPTION_MSG);
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**-------------------------
     * Sửa thông tin NVL
     * CreatedBy: LNT (12/10)
     */
    async editInventoryItem(){
      var self = this;
      try {
        await axios
          .put(`${URL}/${self.inventoryItemId}`, self.inventoryItem)
          .then((res) => {
            if (res.data.StatusCode == STATUS_CODE.SUCCESS) {
              self.$toast.success(MESSAGE.EDIT_MSG_SUCCESS);
              console.log(res);
              // Object.assign(this.employeeOriginalEdit, this.employee);
            } else {
              this.$toast.error(MESSAGE.EXCEPTION_MSG);
            }
          })
          .catch((error) => {
            console.log(error);
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**------------------------
     * Sinh mã mới
     * CreatedBy: LNT (13/10)
     */
    async autoNewCode(value) {
      let code = '';
      try {
        const result  = await axios.get(URL + `/NewCode?code=` + value);
        code = result.data.Data;
      } catch (error) {
        console.log(error);
      }
      return code;
    },
    /**---------------------------
     * Lấy tất cả bản ghi Item
     * CreatedBy: LNT (02/09)
     */
    async getAllData() {
      let objects = [];
      try {
        const result  = await axios.get(URL);
        if (result.data.StatusCode == STATUS_CODE.SUCCESS) {
          objects = result.data.Data;
        } else {
          objects = [];
          this.$toast.error(MESSAGE.EXCEPTION_MSG);
        }
      } catch (error) {
        console.log(error);
        this.$toast.error(MESSAGE.EXCEPTION_MSG);
      }
      return objects;
    },
  },
};
</script>

<style scoped>
@import url(../../css/layout/inventoryForm.css);

#help {
  width: 84px;
  margin-left: 12px;
  outline: none;
}

#save {
  width: 70px;
  margin-right: 8px;
  outline: none;
}

#saveAs {
  width: 130px;
  margin-right: 8px;
  outline: none;
}

#cancel {
  width: 96px;
  margin-right: 12px;
  outline: none;
}

#addRow {
  width: 126px;
  margin-left: 12px;
  outline: none;
}

#deleteRow {
  width: 113px;
  margin-left: 8px;
  outline: none;
}

.hideForm {
  display: none;
}

.border-error{
  border-color: red;
}

.hide-form-error{
  display: none;
}
</style>