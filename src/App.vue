<template>
  <div id="app">
    <TheMenu/>
    <TheHeader/>
    <TheContent/>
  </div>
</template>

<script>
import TheMenu from './views/TheMenu.vue';
import TheHeader from './views/TheHeader.vue';
import TheContent from './views/TheContent.vue';

export default {
  name: 'App',
  components: {
    TheMenu,
    TheHeader,
    TheContent,
  }
}
</script>

<style>
@import url(./css/main.css);


</style>
