<template>
    <div class="content-contain">
        <div class="title-bar">
            <div class="text">Nguyên vật liệu</div>
            <button 
                class="button"
                id="help"
            >
                <div class="button-icon icon-help"></div>
                <div class="button-text">Phản hồi</div>
            </button>
        </div>
        <div class="table-paging-contain">
            <InventoryList/>
        </div>
    </div>
</template>

<script>
import InventoryList from '../components/layout/InventoryList.vue';
export default {
    components:{
        InventoryList,
    }
}
</script>

<style scoped>
@import url(../css/view/content.css);
#help{
    position: absolute;
    right: 8px;
    width: 110px;
    bottom: 0;
    outline-color: #0072BC;
    outline: 1px;
}
</style>