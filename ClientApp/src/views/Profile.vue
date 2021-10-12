<template>
    <div class="container p-3">
        <div class="row">
            <div class="col-2">
            <img :src="$auth.user.picture" />
        </div>

        <div class="col-10">
            <h2>{{ $auth.user.name }}</h2>
            <p>{{ $auth.user.email }}</p>
            <pre>{{ JSON.stringify($auth.user, null, 2) }}</pre>
        </div>
        </div>
    </div>
</template>