<template>
  <div id="app">
    <TheMenu/>
    <TheHeader/>
    <TheContent style="display: block;"/>
    <!-- <InventoryForm style="display: block;"/> -->
    <Popup/>
  </div>
</template>

<script>
import TheMenu from './views/TheMenu.vue';
import TheHeader from './views/TheHeader.vue';
import TheContent from './views/TheContent.vue';
// import InventoryForm from './components/layout/InventoryForm.vue';
import Popup from './components/base/Popup.vue';

export default {
  name: 'App',
  components: {
    TheMenu,
    TheHeader,
    TheContent,
    // InventoryForm,
    Popup,
  }
}
</script>

<style>
@import url(./css/main.css);


</style>
