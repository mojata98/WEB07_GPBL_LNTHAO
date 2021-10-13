<template>
    <div class="table-hold">
        <table >
            <thead>
                <tr>
                    <th class="tb-border-left" style="width: 140px; padding-left: 10px; padding-right: 10px;">Mã nguyên vật liệu</th>
                    <th
                        style="padding-left: 10px; padding-right: 10px;"
                        v-for="(item, index) in headers"
                        :key="index"
                        :style="{ 'width': item.width }"
                    >
                        {{ item.text }}
                    </th>
                </tr>
                <tr>
                    <th class="tb-border-left">
                        <div style="display: flex; align-item: center;">
                        <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        />
                        <input type="text" class="input input-header" style="width: 100%;">
                        </div>
                    </th>
                    <th >
                        <div style="display: flex;">
                        <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        />
                        <input type="text" class="input input-header" style="width: 100%;">
                        </div>
                    </th>
                    <th >
                        <div style="display: flex;">
                        <!-- <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        /> -->
                        <Combobox
                            type="tableList"
                            ref="inputProperties"
                            id="txtProperties"
                            fieldType="properties"
                            displayName="Tính chất"
                            value=""
                            itemId="PropertiesOfMaterialsId"
                            itemName="PropertiesOfMaterialsName"
                            @inputName="filterByProperty"
                        />
                        <!-- v-if="item.fieldname == 'PropertiesOfMaterialName'" -->
                        <!-- <input type="text" class="input input-header" style="width: 130px;"> -->
                        </div>
                    </th>
                    <th >
                        <div style="display: flex;">
                        <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        />
                        <input type="text" class="input input-header" style="width: 100%;">
                        </div>
                    </th>
                    <th >
                        <div style="display: flex;">
                        <!-- <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        />
                        <input type="text" class="input input-header" style="width: 100%;"> -->
                        <Combobox
                            type="tableList"
                            ref="inputInventoryItemGroupName"
                            id="txtInventoryItemGroupName"
                            fieldType="inventoryItemGroupName"
                            displayName="Nhóm nguyên vật liệu"
                            value=""
                            itemId="InventoryItemGroupId"
                            itemName="InventoryItemGroupName"
                            @inputName="filterByInventoryItemGroupName"
                        />
                        </div>
                    </th>
                    <th >
                        <div style="display: flex;">
                        <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        />
                        <input type="text" class="input input-header" style="width: 100%;">
                        </div>
                    </th>
                    <th class="tb-border-right">
                        <div style="display: flex;">
                        <!-- <FilterDropdown
                            :data="dataFilter"
                            ref="textFilterDropdown"
                        />
                        <input type="text" class="input input-header" style="width: 100%;"> -->
                        <Combobox
                            type="tableList"
                            ref="inputStopUsing"
                            id="txtStopUsing"
                            fieldType="stopUsing"
                            displayName="Ngưng sử dụng"
                            value=""
                            itemId="StopUsing"
                            itemName="StopUsingName"
                            
                        />
                        <!-- v-if="item.fieldname == 'StopUsing'" -->
                        </div>
                    </th>
                </tr>
            </thead>

            <tbody>
            <!-- <tr>
                <td class="tb-border-left">BIAHANOI</td>
                <td>Bia Hà Nội</td>
                <td>Đồ uống đóng chai</td>
                <td>Chai</td>
                <td></td>
                <td></td>
                <td><div class="block-center"><input type="checkbox" disabled></div></td>
            </tr> -->
            <tr
                v-for="(inventoryItem) in inventoryItems" :key="inventoryItem.InventoryItemId"
                @click="focusRowTable(inventoryItem)"
                @dblclick="fixInfor(inventoryItem.InventoryItemId)"
            >
            <!-- @dblclick="fixInfor(employee.EmployeeId)" -->
                <!--Cột 1: Mã nguyên vật liệu-->
                <td class="tb-border-left">
                    {{ inventoryItem.InventoryItemCode }}
                </td>
                <!--Cột 2: Tên nguyên vật liệu-->
                <td>
                    {{ inventoryItem.InventoryItemName }}
                </td>
                <!--Cột 3: Tính chất-->
                <td>
                    {{ inventoryItem.PropertyOfInventoryItem }}
                </td>
                <!--Cột 4: Đơn vị tính-->
                <td>
                    {{ inventoryItem.UnitName }}
                </td>
                <!--Cột 5: Nhóm nguyên vật liệu-->
                <td>
                    {{ inventoryItem.InventoryItemGroupName }}
                </td>
                <!--Cột 6: Ghi chú-->
                <td>
                    {{ inventoryItem.CreatedBy }}
                </td>
                <!--Cột 7: Ngừng theo dõi-->
                <td>
                    {{ formatStopWorking(inventoryItem.StopWorking) }}
                </td>
            </tr>
            <tr v-if="inventoryItems.length == 0 ? true : false">
                <td colspan="7" style="text-align: center">
                    Không có dữ liệu hiển thị.
                </td>
            </tr>
        </tbody>
        </table>

        
    </div>
</template>

<script>
import FilterDropdown from '../base/FilterDropdown.vue';
import eventBus from '../../eventBus';
import Combobox from '../base/Combobox.vue';
export default {
    components:{
        FilterDropdown,
        Combobox,
    },
    data() {
        return {
            // data cho filter-dropdown 
            dataFilter: [
                { Text: "* : Chứa", Value: 1 , Sign: "*"},
                { Text: "= : Bằng", Value: 2 , Sign: "="},
                { Text: "+ : Bắt đầu bằng", Value: 3 , Sign: "+"},
                { Text: "- : Kết thúc bằng", Value: 4 , Sign: "-"},
                { Text: "! : Không chứa", Value: 5 , Sign: "!"},
            ],
            clickRow: false, // focus vào 1 hàng trên bảng

            inventoryItemId: "", // Id NVL
        }
    },
    props: {
        headers: {
            type: Array,
        },
        inventoryItems: {},// mảng nguyên vật liệu
    },
    methods: {
        /**------------------------
         * Format StopWorking
         * CreatedBy: LNT (03/10)
         */
        formatStopWorking(value){
            if (value == 0){
                return "Ngưng sử dụng";
            }
            else{
                return "Đang sử dụng";
            }
        },
        /**---------------------------
         * Nhấn chuột vào 1 hàng bất kỳ
         * CreatedBy: LNT (08/10)
         */
        focusRowTable(value){
            this.clickRow = !this.clickRow;
            this.$emit("passObject", value);
        },
        /**-----------------------------------
         * Nhấn chuột 2 lần vào 1 hàng bất kỳ
         * CreatedBy: LNT (08/10)
         */
        fixInfor(id){
            this.inventoryItemId = id;
            eventBus.$emit("edit", this.inventoryItemId);
            eventBus.$emit("openForm");
        },
        filterByInventoryItemGroupName(value){
            this.$emit("inputFilterByInventoryItemGroupName", value);
            console.log(value);
        },
        /**--------------------------
         * Filter data theo tính chất
         * CreatedBy: LNt (27/8)
         */
        filterByProperty(value){
            this.$emit("inputFilterByProperty", value);
        },
        /**------------------------
         * Reset combobox
         * CreatedBy: LNT (12/10)
         */
        resetCombobox(){
            this.$refs.inputInventoryItemGroupName.setTextDefault();
            this.$refs.inputProperties.setTextDefault();
            this.$refs.inputStopUsing.setTextDefault();
        }
    },
}
</script>

<style scoped>
@import url(../../css/base/table.css);

tbody tr.active{
    background-color: #f3f8f8 !important;
}

#txtProperties{
    height: 26px;
}
</style>